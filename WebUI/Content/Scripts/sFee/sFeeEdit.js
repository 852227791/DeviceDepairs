
var year = 2015;
var sOrderID = "0";
var tempShoudMoney = "0";
var isSelect = false;

var selectGrid;
var selectiIndex = 0;
var receivedMoney = 0;
var allDataGrid = [];
var tempOffset = [];


bindFormEvent = function () {
    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300,
        value: defaultDeptID,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#DeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            $("#sEnrollsProfessionID").textbox("setValue", "");
            $("#sEnrollText").textbox("setValue", "");
            deptId = node.id;
            closeAll();
        }
    });
    setTimeout(function () {
        $('#ExamNum').textbox('textbox').focus();//考生号获取焦点
        $('#ExamNum').textbox('textbox').keydown(function (e) {//回车事件
            if (e.keyCode == 13) {
                var examNum = $('#ExamNum').textbox('getValue');
                var deptId = $("#DeptID").combotree("getValue");
                if (deptId === "" || deptId === null) {
                    Easy.centerShow("系统消息", "请先选择收费单位", 2000);
                    return false;
                }
                if (examNum != "" && examNum != null) {
                    var result = Easy.bindSelectInfomation("../sOrder/SelectPlan", { ExamNum: examNum, DeptID: deptId });
                    if (result.IsError === false) {
                        var data = JSON.parse(result.Data);
                        PlanItemID = data[0].ItemID;
                        $("#sEnrollsProfessionID").textbox("setValue", data[0].sEnrollsProfessionID);
                        var result1 = Easy.bindSelectInfo("../sEnroll/GetPlanName", data[0].sEnrollsProfessionID);
                        var name = JSON.parse(result1.Data);
                        $("#sEnrollText").textbox("setValue", name[0].PlanName);
                        if (name[0].StudyNum != "" && name[0].StudyNum != null) {
                            $("#Explain").textbox("setValue", "教务学号：" + name[0].StudyNum + "");
                        }

                        $("#sEnrollID").textbox("setValue", name[0].sEnrollID);
                        closeAll();//关闭所有标签   
                        isSelect = false;//初始化
                        bindOrderEvent(data[0].ItemID, data[0].sEnrollsProfessionID);//加载订单tabs
                        bindGiveEvent(data[0].ItemID, data[0].sEnrollsProfessionID);//加载配品tabs
                        setTimeout(function () {
                            bindLoadsOrderCombobox(data[0].ItemID, data[0].sEnrollsProfessionID);//加载缴费次数Combobox
                            bindDiscountCombobox(data[0].ItemID);
                        }, 1);
                        $('#tabsFee').tabs('select', 0);//设置选中第几个选项卡
                    }
                    else {
                        closeAll();
                        Easy.centerShow("系统消息", result.Message, 2000);
                    }
                }
                $.messager.progress('close');
            }
        });
    }, 1);

    $("#FeeTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });
    $("#FeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false
    });
    $("#ShouldMoney").numberspinner({
        readonly: true
    });
    //$("#ItemID").combobox({
    //    url: "../Refe/SelList?RefeTypeID=11",
    //    valueField: "Value",
    //    textField: "RefeName",
    //    editable: false,
    //    onChange: function (newValue, oldValue) {
    //        closeAll();
    //        bindOrderEvent(newValue);
    //        bindGiveEvent(newValue);
    //        $('#tabsFee').tabs('select', 0);//设置选中第几个选项卡
    //    }
    //});
    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "feetime")) {//编辑收费时间权限控制
        $("#FeeTime").datebox({
            readonly: true
        });
    }
}

bindOrderEvent = function (ItemID, sEnrollsProfessionID) {//根据方案ID异步生成
    var sOrderData = Easy.bindSelectInfomation("../sOrder/GetOrderInfomation", { ItemID: ItemID, sEnrollsProfessionID: sEnrollsProfessionID });
    var item = JSON.parse(sOrderData.Data);
    gridText = "";
    for (var i = 0; i < item.length; i++) {
        var grid = "feeGrid" + item[i].NumItemID
        gridText += grid + ",";
        var tempTitle = '' + item[i].NumName;// + '(' + item[i].LimitTime + ')'
        var flag = false;
        var data = JSON.parse(Easy.bindSelectInfomation("../sOrder/GetsOrderList", { sEnrollsProfessionID: sEnrollsProfessionID, NumItemID: item[i].NumItemID, ItemID: ItemID }).Data);
        var limitTime = "";
        for (var j = 0; j < data.rows.length; j++) {
            if (j === 0) {
                limitTime = "(" + data.rows[j].LimitTime + ")";
            }

            var uppaid = parseFloat(data.rows[j].UnPaid);
            if (uppaid != 0) {
                flag = true;
                break;
            }
        }
        tempTitle += limitTime ;
        var temp = false;
        if (!flag) {
            tempTitle = tempTitle + "(已缴清)";
            temp = true;
        }
        $('#tabsFee').tabs('add', {
            title: tempTitle,
            content: '<table id="' + grid + '" data-options="border:false" style="height:268px"></table>'
        });
        bindOrderListEvent("#" + grid + "", data, tempTitle, temp);

    }

    $('#tabsFee').tabs({
        onSelect: function (title, index) {
            var gridTextTemp = gridText.substring(0, gridText.length - 1);
            var gridArr = gridTextTemp.split(',');
            for (var i = 0; i < gridArr.length; i++) {
                $("#" + gridArr[i]).edatagrid("saveRow");
            }
        }
    });
}

bindGiveEvent = function (ItemID, sEnrollsProfessionID) {//根据方案ID异步生成
    var giveData = Easy.bindSelectInfomation("../sItemsGive/GetGiveByItem", { ItemID: ItemID, sEnrollsProfessionID: sEnrollsProfessionID });
    var item = JSON.parse(giveData.Data);
    $('#tabsFee').tabs('add', {
        title: '配品',
        content: '<div id="a"  class="easyui-layout" style="height:268px"> <div style="margin-top: 6px; margin-left: 6px;height:25px "data-options="region:\'north\',border:false"> 配品归属缴费次数：<input class="easyui-combobox" type="text" id="selNumItemID" name="selNumItemID" style="width:160px;" /></div> <div data-options="region:\'center\',border:false"><table id="giveGrid" data-options="border:false" style="height:267px"></table></div> </div>'
    });
    bindGiveListEvent("#giveGrid", item);
}

bindOrderListEvent = function (grid, orData, title, temp) {
    $(grid).edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        showFooter: true,
        data: orData,
        columns: [[
            { field: "sOrderID", hidden: true },
            { field: "DetailName", title: "收费类别", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应收金额", sortable: true, halign: 'left', align: 'right' },
            { field: "ReceivedMoney", title: "已收金额", sortable: true, halign: 'left', align: 'right' },
            { field: "UnPaid", title: "未缴金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实收金额", width: 80, sortable: true, halign: 'left', align: 'right', editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0, onChange: function (newValue, oldValue) { bindSumShouldMoneyEvent(gridText, newValue, oldValue); } } } },
            { field: "DiscountMoney", title: "优惠金额", width: 80, sortable: true, halign: 'left', align: 'right', editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0, onChange: function (newValue, oldValue) { bindSumDiscountMoneyEvent(gridText, newValue, oldValue); } } } },
            { field: "OffsetMoney", title: "充抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "OffsetDetail", hidden: true },//width: 800, editor: { type: 'text' }
            {
                field: 'OffsetBtn', title: '操作', width: 100, align: 'center',
                formatter: function (value, row, index) {
                    if (JSON.stringify(row) != "{}") {
                        if (row.ShouldMoney.toString().indexOf('合计') < 0) {
                            var btn = "";
                            //if (!temp)
                            //    btn += "<a onclick=bindOffsetEvent('" + grid + "'," + index + ") href='javascript:void(0)' class='easyui-linkbutton'>选择充抵项</a>";

                            if (row.IsGive === "3") {
                                btn += "&nbsp;&nbsp;<a onclick=bindShouldEvent('" + grid + "'," + index + ") href='javascript:void(0)' class='easyui-linkbutton'>改应收</a>";
                            }
                            if (!temp)
                                btn += "&nbsp;&nbsp;<a onclick=bindDelEvent('" + grid + "'," + index + ") href='javascript:void(0)' class='easyui-linkbutton'>清零</a>";
                            return btn;
                        }
                    }
                }
            }
        ]],
        toolbar: [
            { iconCls: 'icon-edit', text: '全额缴费', handler: function () { bindAllFeeEvent(grid); } },
            { iconCls: 'icon-edit', text: '余额全缴', handler: function () { bindSurplusAllFeeEvent(grid); } },
            { iconCls: 'icon-edit', text: '全额优惠', handler: function () { bindAllDiscount(grid); } },
            { iconCls: 'icon-filter', text: '快速充抵', handler: function () { bindFastsOffset(grid); } },
            { iconCls: 'icon-filter', text: '同费用充抵', handler: function () { bindSameFeeOffset(grid); } },
            { iconCls: 'icon-no', text: '全部清零', handler: function () { bindAllZeroEvent(grid); } }
        ],
        sortName: "sOrderID", sortOrder: "asc",
        onSave: function (index, row) {
            if ((parseFloat(row.OffsetMoney) + parseFloat(row.DiscountMoney) + parseFloat(row.PaidMoney) + parseFloat(row.ReceivedMoney)) > parseFloat(row.ShouldMoney)) {
                Easy.centerShow("系统消息", "" + row.NumName + "" + row.DetailName + "：  实收+充抵+优惠+已收 不能超过" + row.ShouldMoney + "元", 2000);
                return false;
            }
        },
        onDblClickRow: function (rowIndex, rowData) {
            $(grid).edatagrid("saveRow");
            $(grid).edatagrid("editRow", rowIndex);
        },
        onLoadSuccess: function (data) {
            for (var i = 0; i < data.rows.length; i++) {
                var uppaid = parseFloat(data.rows[i].UnPaid);
                if (uppaid != 0) {
                    if (!isSelect) {
                        var tab = $('#tabsFee').tabs('getTab', title);
                        setTimeout(function () { $('#tabsFee').tabs('select', title); }, 1);
                        isSelect = true;
                        break;
                    }
                }
            }
        }
    });
}

bindGiveListEvent = function (grid, item) {
    $(grid).edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        columns: [[
            { field: "sOrderGiveID", checkbox: true },
            { field: "NumItemID", hidden: true },
            { field: "GiveName", title: "配品", width: 200, sortable: true }
        ]],
        data: item,
        sortName: "sItemsGiveID", sortOrder: "asc"
    });
}
//全部缴费
bindAllFeeEvent = function (grid) {
    var data = $(grid).datagrid('getData');
    for (var i = 0; i < data.rows.length; i++) {
        var index = $(grid).datagrid('getRowIndex', data.rows[i]);
        var UnPaid = data.rows[i].UnPaid;
        $(grid).datagrid('updateRow', {
            index: index,
            row: {
                PaidMoney: UnPaid,
                DiscountMoney: "0.00",
                OffsetMoney: "0.00",
                OffsetDetail: "[]"
            }
        });
    }
    bindSumMoneyEvent(gridText, "0", "0");
}
//全部清零
bindAllZeroEvent = function (grid) {
    var data = $(grid).datagrid('getData');
    for (var i = 0; i < data.rows.length; i++) {
        var index = $(grid).datagrid('getRowIndex', data.rows[i]);
        $(grid).datagrid('updateRow', {
            index: index,
            row: {
                PaidMoney: "0.00",
                DiscountMoney: "0.00",
                OffsetMoney: "0.00",
                OffsetDetail: "[]"
            }
        });
    }
    bindSumMoneyEvent(gridText, "0", "0");
}

//打开修改应收金额页
bindShouldEvent = function (grid, index) {
    $(grid).datagrid('selectRow', index);
    var data = $(grid).datagrid('getSelected');
    sOrderID = data.sOrderID;
    tempShoudMoney = data.ShouldMoney;//应缴金额.用于表单赋值
    selectGrid = grid;//选中的grid
    selectiIndex = index;//选中行index
    receivedMoney = data.ReceivedMoney;//已收金额
    Easy.OpenDialogEvent("#changeShouldMoney", "修改应收金额", 400, 250, "../sFee/ChangeShouldMoney", "#changeShouldMoney-buttons");
}

bindDelEvent = function (grid, index) {
    $(grid).datagrid('updateRow', {
        index: index,
        row: {
            PaidMoney: "0.00",
            DiscountMoney: "0.00",
            OffsetMoney: "0.00",
            OffsetDetail: "[]"
        }
    });
    bindSumMoneyEvent(gridText, "0", "0");
}

bindSumMoneyEvent = function (gridText, newValue, oldValue) {
    var sum = 0;
    var discount = 0;
    var offsetMoney = 0;
    gridTextTemp = gridText.substring(0, gridText.length - 1);
    var gridArr = gridTextTemp.split(',');

    for (var i = 0; i < gridArr.length; i++) {
        var data = $("#" + gridArr[i]).datagrid('getData');
        for (var j = 0; j < data.rows.length; j++) {
            sum += parseFloat(data.rows[j].PaidMoney);
            discount += parseFloat(data.rows[j].DiscountMoney);
            offsetMoney += parseFloat(data.rows[j].OffsetMoney);
        }
    }
    //if (oldValue != "") {
    //    sum = parseFloat(sum) + parseFloat(newValue) - parseFloat(oldValue);
    //}
    $("#ShouldMoney").numberspinner("setValue", sum);
    $("#offsetTotal").numberspinner("setValue", offsetMoney);
    $("#discountTotal").numberspinner("setValue", discount);
}

bindSumShouldMoneyEvent = function (gridText, newValue, oldValue) {
    var sum = $("#ShouldMoney").numberspinner("getValue");
    if (sum === "") {
        sum = 0;
    }
    if (oldValue != "") {
        sum = parseFloat(sum) + parseFloat(newValue) - parseFloat(oldValue);
    }
    $("#ShouldMoney").numberspinner("setValue", sum);
}

bindSumDiscountMoneyEvent = function (gridText, newValue, oldValue) {
    var discount = $("#discountTotal").numberspinner("getValue");
    if (discount === "") {
        discount = 0;
    }
    if (oldValue != "") {
        discount = parseFloat(discount) + parseFloat(newValue) - parseFloat(oldValue);
    }
    $("#discountTotal").numberspinner("setValue", discount);
}

//关闭所有标签
function closeAll() {
    $("#ShouldMoney").textbox("setValue", "");
    $("#offsetTotal").textbox("setValue", "0.00");
    $("#discountTotal").textbox("setValue", "0.00");
    $('#tabsFee').tabs({
        onSelect: function (title, index) {
        }
    });
    $("#tabsFee .tabs li").each(function (index, obj) {
        var tab = $(".tabs-title", this).text();
        $(".easyui-tabs").tabs('close', tab);
    });
}
//选择方案选择事件
bindChoseFeePlanMethod = function () {
    $("#btnChooseFeePlan").click(function () {
        var dept = $("#DeptID").combotree('getValue');
        if (dept === null || dept === "") {
            Easy.centerShow("系统消息", "请选择收费单位", 3000);
            return false;
        }
        else {
            deptId = dept;
            Easy.OpenDialogEvent("#choosesFeePlan", "选择缴费方案", 780, 580, "../sFee/ChooseFeePlan", "#choosesFeePlan-buttons");
        }
    });
};

bindLoadsOrderCombobox = function (planItemId, proId) {
    $("#selNumItemID").combobox({
        url: "../sOrder/GetsOrderCombobox",
        valueField: "id",
        textField: "name",
        queryParams: { PlanItemID: planItemId, ProID: proId },
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (record) {
            var data = $("#giveGrid").datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                var index = $("#giveGrid").datagrid("getRowIndex", data.rows[i]);
                bindUpdateGiveDataGridRows(record.id, index);
            }
        }
    });
}

bindUpdateGiveDataGridRows = function (numitemId, index) {
    $("#giveGrid").datagrid('updateRow', {
        index: index,
        row: {
            NumItemID: numitemId
        }
    });
};


//选择充抵项
//bindOffsetEvent = function (grid, index) {
//    totalData = [];
//    allData = [];
//    $(grid).datagrid("selectRow", index);
//    selectGrid = grid;//选中的grid
//    selectiIndex = index;//选中行index
//    var data = $(grid).datagrid("getSelected");
//    var gridArray = gridText.substring(0, gridText.length - 1).split(",");
//    for (var i = 0; i < gridArray.length; i++) {
//        var data1 = $("#" + gridArray[i]).datagrid("getData");
//        for (var k = 0; k < data1.rows.length; k++) {
//            var temp = JSON.parse(data1.rows[k].OffsetDetail);
//            for (var j = 0; j < temp.length; j++) {
//                totalData.push(temp[j]);
//            }
//        }
//    }
//    allData = JSON.parse(data.OffsetDetail);
//    Easy.OpenDialogEvent("#chooseFeeOffsetItem", "选择充抵项", 780, 590, "../sFee/ChooseFeeOffsetItem", "#chooseFeeOffsetItem-buttons");
//}
///保存充抵项
//bindSaveoffsetMethod = function () {
//    $("#btnSave_ChooseFeeOffsetItem").click(function () {
//        var offsetdata = $("#sfeeOffset").datagrid("getData");
//        if (offsetdata.rows.length === 0) {
//            Easy.centerShow("系统消息", "没有选择任何充抵!", 2000);
//            return false;
//        }
//        var tempMoney = 0;
//        var tempData = [];
//        for (var i = 0; i < offsetdata.rows.length; i++) {
//            tempMoney += parseFloat(offsetdata.rows[i].Money);
//            var temp = {
//                Sort: offsetdata.rows[i].Sort, ID: offsetdata.rows[i].ID, Money: offsetdata.rows[i].Money, VoucherNum: offsetdata.rows[i].VoucherNum,
//                NoteNum: offsetdata.rows[i].NoteNum, IDCard: offsetdata.rows[i].IDCard, StudName: offsetdata.rows[i].StudName, FeeContent: offsetdata.rows[i].Content,
//                Dept: offsetdata.rows[i].Dept, FeeTime: offsetdata.rows[i].FeeTime, OffsetItem: offsetdata.rows[i].OffsetItem, sOrderID: offsetdata.rows[i].sOrderID
//            };
//            tempData.push(temp);
//        }

//        tempMoney = tempMoney.toFixed("2");
//        $(selectGrid).datagrid('updateRow', {
//            index: selectiIndex,
//            row: {
//                OffsetMoney: tempMoney,
//                OffsetDetail: JSON.stringify(tempData)
//            }
//        });
//        $("#chooseFeeOffsetItem").dialog('close');
//        bindSumMoneyEvent(gridText, "0", "0");
//    });
//}
//快速充抵
bindFastsOffset = function (grid) {
    allDataGrid = [];
    totalData = [];
    var gridArray = gridText.substring(0, gridText.length - 1).split(",");
    for (var i = 0; i < gridArray.length; i++) {
        var data1 = $("#" + gridArray[i]).datagrid("getData");
        for (var k = 0; k < data1.rows.length; k++) {
            var temp = JSON.parse(data1.rows[k].OffsetDetail);
            for (var j = 0; j < temp.length; j++) {
                totalData.push(temp[j]);
            }
        }
    }
    allDataGrid = $(grid).datagrid("getData");
    selectGrid = grid;
    //Easy.OpenDialogEvent("#fastsOffset", "快速充抵", 800, 600, "../sFee/ChooseFastsOffset", "#fastsOffset-buttons"); 
    Easy.OpenDialogEvent("#quicksOffset", "快速充抵", 800, 600, "../sFee/ChooseQuicksOffset", "#quicksOffset-buttons");
}
//同费用
var sfeeOrderdata;
bindSameFeeOffset = function (grid) {
    var numItemId = grid.replace("#feeGrid", "");
    var senrollsPorfessionId = $("#sEnrollsProfessionID").textbox("getValue");
    var result = Easy.bindSelectInfomation("../sEnroll/ValidateIsSame", { PlanItemID: PlanItemID, NumItemID: numItemId, sEnrollsProfessionID: senrollsPorfessionId });
    if (result.IsError === false) {
        sfeeOrderdata = result.Data;
    }
    else {
        Easy.centerShow("系统消息", result.Message, 2000);
        return false;
    }


    allDataGrid = [];
    totalData = [];
    var gridArray = gridText.substring(0, gridText.length - 1).split(",");
    for (var i = 0; i < gridArray.length; i++) {
        var data1 = $("#" + gridArray[i]).datagrid("getData");
        for (var k = 0; k < data1.rows.length; k++) {
            var temp = JSON.parse(data1.rows[k].OffsetDetail);
            for (var j = 0; j < temp.length; j++) {
                totalData.push(temp[j]);
            }
        }
    }

    allDataGrid = $(grid).datagrid("getData");
    selectGrid = grid;
    Easy.OpenDialogEvent("#quicksOffset", "同费用充抵", 800, 600, "../sFee/SameFeeOffset", "#quicksOffset-buttons");
};

///保存快速充抵项
bindSaveQuickoffsetMethod = function () {
    $("#btnSaveQuicksOffset").click(function () {
        var offsetdata = $("#sFeesOffset").datagrid("getData");

        if (offsetdata.rows.length === 0) {
            Easy.centerShow("系统消息", "没有选择任何充抵，不能保存", 2000);
            return false;
        }
        var data = $(selectGrid).datagrid("getData");
        for (var k = 0; k < data.rows.length; k++) {
            var tempMoney = 0;
            var tempData = [];
            for (var i = 0; i < offsetdata.rows.length; i++) {
                if (data.rows[k].sOrderID === offsetdata.rows[i].sOrderID) {
                    tempMoney += parseFloat(offsetdata.rows[i].Money);
                    var temp = {
                        Sort: offsetdata.rows[i].Sort, ID: offsetdata.rows[i].ID, Money: offsetdata.rows[i].Money, VoucherNum: offsetdata.rows[i].VoucherNum,
                        NoteNum: offsetdata.rows[i].NoteNum, IDCard: offsetdata.rows[i].IDCard, Name: offsetdata.rows[i].Name, FeeContent: offsetdata.rows[i].FeeContent,
                        DeptName: offsetdata.rows[i].DeptName, OffsetItem: offsetdata.rows[i].OffsetItem, sOrderID: offsetdata.rows[i].sOrderID
                    };
                    tempData.push(temp);
                    //  tempOffset.push(offsetdata.rows[i]);
                }
            }
            if (tempData.length > 0) {
                var tempString = JSON.stringify(tempData);
                tempMoney = tempMoney.toFixed("2");
                $(selectGrid).datagrid('updateRow', {
                    index: k,
                    row: {
                        OffsetMoney: tempMoney,
                        OffsetDetail: tempString
                    }
                });
            }

        }

        $("#quicksOffset").dialog('close');
        bindSumMoneyEvent(gridText, "0", "0");
    });
}

bindAllDiscount = function (grid) {
    var data = $(grid).datagrid('getData');
    for (var i = 0; i < data.rows.length; i++) {
        var index = $(grid).datagrid('getRowIndex', data.rows[i]);
        var UnPaid = data.rows[i].UnPaid;
        $(grid).datagrid('updateRow', {
            index: index,
            row: {
                PaidMoney: "0.00",
                DiscountMoney: UnPaid,
                OffsetMoney: "0.00",
                OffsetDetail: "[]"
            }
        });
    }
    bindSumMoneyEvent(gridText, "0", "0");
};
bindSurplusAllFeeEvent = function (girdId) {
    var data = $(girdId).datagrid('getData');

    for (var i = 0; i < data.rows.length; i++) {
        var index = $(girdId).datagrid('getRowIndex', data.rows[i]);
        if (data.rows[i].ReceivedMoney === undefined) {
            data.rows[i].ReceivedMoney = "0";
        }
        var shouldMoney = data.rows[i].ShouldMoney;
        var receivedMoney = data.rows[i].ReceivedMoney;
        var discountMoney = data.rows[i].DiscountMoney;
        var offsetMoney = data.rows[i].OffsetMoney;
        var UnPaid = parseFloat(shouldMoney) - (parseFloat(receivedMoney) + parseFloat(discountMoney) + parseFloat(offsetMoney));
        if (parseFloat(UnPaid) < 0) {
            UnPaid = "0.00";
        }
        $(girdId).datagrid('updateRow', {
            index: index,
            row: {
                PaidMoney: parseFloat(UnPaid).toFixed("2")
            }
        });
    }
    bindSumMoneyEvent(gridText, "0", "0");
}
bindDiscountCombobox = function (itemId) {
    $("#discountPlan").combobox({
        url: "../DiscountPlan/GetDiscountPlanCombobox",
        valueField: "id",
        textField: "text",
        queryParams: { ItemID: itemId },
        panelHeight: 120,
        multiple: false,
        editable: false,
        value: "0",
        onChange: function (newVlaue, oldValue) {
            bindSetDiscountNull();
            if (newVlaue != "0") {
                var result = Easy.bindSelectInfo("../DiscountPlan/GetDiscountDetail", newVlaue);
                var data = JSON.parse(result.Data);//获取优惠明细
                var gridTextTemp = gridText.substring(0, gridText.length - 1);
                var gridArr = gridTextTemp.split(',');
                for (var i = 0; i < gridArr.length; i++) {
                    var itemId = gridArr[i].replace("feeGrid", "");
                    var girdData = $("#" + gridArr[i]).datagrid("getData");
                    for (var k = 0; k < girdData.rows.length; k++) {
                        for (var j = 0; j < data.length; j++) {
                            if (girdData.rows[k].ItemDetailID === data[j].ItemDetaiID && itemId === data[j].NumItemID) {
                                var rowIndex = $("#" + gridArr[i]).datagrid("getRowIndex", girdData.rows[k]);
                                $("#" + gridArr[i]).datagrid("updateRow", { index: rowIndex, row: { "DiscountMoney": data[j].Money } });
                            }
                        }
                    }

                }

            }
            bindSumMoneyEvent(gridText, "0", "0");
        }
    });
}

bindSetDiscountNull = function () {
    var gridTextTemp = gridText.substring(0, gridText.length - 1);
    var gridArr = gridTextTemp.split(',');
    for (var i = 0; i < gridArr.length; i++) {
        var girdData = $("#" + gridArr[i]).datagrid("getData");
        for (var k = 0; k < girdData.rows.length; k++) {
            var rowIndex = $("#" + gridArr[i]).datagrid("getRowIndex", girdData.rows[k]);
            $("#" + gridArr[i]).datagrid("updateRow", { index: rowIndex, row: { "DiscountMoney": "0.00" } });
        }
    }
}

//bindSaveoffsetMethod();
bindFormEvent();
bindChoseFeePlanMethod();
bindSaveQuickoffsetMethod();

