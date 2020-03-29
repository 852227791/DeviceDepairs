var index = 0;
var year = 2015;
var sOrderID = "0";
var tempShoudMoney = "0";
var isSelect = false;
bindFormEvent = function () {
    $("#editDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: "7",
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#editDeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            deptId = node.id;
        }
    });

    $("#editFeeTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });
    $("#editFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
    $("#editShouldMoney").numberspinner({
        readonly: true
    });;
    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "feetime")) {//编辑收费时间权限控制
        $("#editFeeTime").datebox({
            readonly: true
        });
    }
}
bindGiveEvent = function (itemId, sEnrollsProfessionID) {//根据方案ID异步生成
    var giveData = Easy.bindSelectInfomation("../sItemsGive/GetGiveByItem", { ItemID: itemId, sEnrollsProfessionID: sEnrollsProfessionID });
    var item = JSON.parse(giveData.Data);
    $('#tabseditFee').tabs('add', {
        title: '配品',
        content: '<table id="editgiveGrid" data-options="border:false" style="height:301px"></table>'
    });
    bindGiveListEvent("#editgiveGrid", item);
}

bindOrderListEvent = function (orData) {
    $("#ordergird").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        showFooter: true,
        data: orData,
        columns: [[
            { field: "sFeesOrderID", hidden: true },
            { field: "DetailName", title: "收费类别", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应收金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实收金额", width: 80, sortable: true, halign: 'left', align: 'right' },//, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0, onChange: function (newValue, oldValue) { bindSumMoneyEvent(newValue, oldValue); } } }
            { field: "DiscountMoney", title: "优惠金额", width: 80, sortable: true, halign: 'left', align: 'right' },//, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0, onChange: function (newValue, oldValue) { bindSumDiscountMoneyEvent(newValue, oldValue); } } }
            { field: "OffsetMoney", title: "充抵金额", sortable: true, halign: 'left', align: 'right' },
            { field: "OffsetDetail", hidden: true }
            //{
            //    field: 'OffsetBtn', title: '操作', width: 150, align: 'center',
            //    formatter: function (value, row, index) {
            //        if (JSON.stringify(row) != "{}") {
            //            if (row.ShouldMoney.toString().indexOf('合计') < 0) {
            //                var btn = "";
            //                btn += "<a onclick=bindModfiyOffsetEvent(" + index + ") href='javascript:void(0)' class='easyui-linkbutton'>选择充抵项</a>";
            //                return btn;
            //            }
            //        }
            //    }
            //}
        ]],
        sortName: "sFeesOrderID", sortOrder: "asc",
        toolbar: [
            //{
            //iconCls: 'icon-no',
            //text: '删除收费明细',
            //handler: function () {
            //    var rows = $("#ordergird").datagrid("getSelections");//选中的所有ID
            //    if (Easy.checkRow(rows, "收费明细")) {
            //        $.messager.confirm("", "确定要删除吗？", function (yes) {
            //            if (yes) {
            //                var result = Easy.bindSelectInfomation("../sFee/DeletesFeesOrder", { sfeesOrderId: rows[0].sFeesOrderID });
            //                if (result.IsError === false) {
            //                    var rowIndex = $("#ordergird").datagrid("getRowIndex", rows[0]);
            //                    $("#ordergird").datagrid("deleteRow", rowIndex);
            //                    Easy.centerShow("系统消息", result.Message, 2000);

            //                }
            //                else {
            //                    Easy.centerShow("系统消息", result.Message, 2000);
            //                }
            //            }
            //        });
            //    }
            //}
            //}, '-',
       {
           iconCls: 'icon-redo',
           text: '全部清零',
           handler: function () {
               var tempData = $("#ordergird").datagrid("getData");
               for (var i = 0; i < tempData.rows.length; i++) {
                   var row = tempData.rows[i];
                   var index = $("#ordergird").datagrid("getRowIndex", row);
                   $('#ordergird').datagrid('updateRow', {
                       index: index,
                       row: {
                           PaidMoney: '0.00',
                           DiscountMoney: '0.00'
                       }
                   });
               }
               bindSumMoneyEvent("0", "0")
           }
       }, '-', {
           iconCls: 'icon-no',
           text: '删除充抵项',
           handler: function () {
               var rows = $("#ordergird").datagrid("getSelections");//选中的所有ID
               if (Easy.checkRow(rows, "收费明细")) {
                   $.messager.confirm("", "确定要删除吗？", function (yes) {
                       if (yes) {
                           var result = Easy.bindSelectInfomation("../sFee/DeletesOffset", { sfeesOrderId: rows[0].sFeesOrderID });
                           if (result.IsError === false) {
                               var rowIndex = $("#ordergird").datagrid("getRowIndex", rows[0]);
                               $('#ordergird').datagrid('updateRow', {
                                   index: rowIndex,
                                   row: {
                                       OffsetMoney: "0.00"
                                   }
                               });
                               Easy.centerShow("系统消息", result.Message, 2000);
                           }
                           else {
                               Easy.centerShow("系统消息", result.Message, 2000);
                           }
                       }
                   });
               }
           }
       }],
    });
}

bindGiveListEvent = function (grid, item) {
    $(grid).edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        columns: [[
            { field: "sItemsGiveID", checkbox: true },
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
bindFormEvent();
//选择方案选择事件
bindSelectInfomation = function () {
    var restult = Easy.bindSelectInfomation("../sFee/GetsFeeInfomation", { sFeeID: sFeeID });
    var data = JSON.parse(restult.Data)[0];

    setTimeout(function () {
        $("#editsFeeID").textbox("setValue", data.sFeeID);
        $("#editsEnrollsProfessionID").textbox("setValue", data.sEnrollsProfessionID);
        $("#editDeptID").combotree("setValue", data.DeptID);
        $("#editFeeTime").datebox("setValue", Easy.bindSetTimeFormatEvent(data.FeeTime));
        $("#editsEnrollText").textbox("setValue", data.PlanName);
        $("#editFeeMode").combobox("setValue", data.FeeMode);
        $("#editDiscountMoney1").numberspinner("setValue", data.DiscountMoney);
        $("#editOffsetMoney").numberspinner("setValue", data.OffsetMoney);
        $("#editShouldMoney").numberspinner("setValue", data.ShouldMoney);
        $("#editExplain").textbox("setValue", data.Explain.replace(/<br \/>/g, "\r\n"));
        $("#editRemark").textbox("setValue", data.Remark.replace(/<br \/>/g, "\r\n"));
        var orderData = JSON.parse(Easy.bindSelectInfomation("../sFeesOrder/GetsOrderList", { ID: sFeeID }).Data);
        var flag = false;
        for (var j = 0; j < orderData.rows.length; j++) {
            var uppaid = parseFloat(orderData.rows[j].UnPaid);
            if (uppaid != 0) {
                flag = true;
            }
        }
        var tempTitle = '' + data.NumName + '(' + data.Year + '年' + data.Month + '月)';
        var temp = false;
        if (!flag) {
            tempTitle = tempTitle + "(已缴清)";
            temp = true;
        }
        $('#tabseditFee').tabs('add', {
            title: tempTitle,
            content: '<table id="ordergird" data-options="border:false" style="height:301px"></table>'
        });
        bindGiveEvent(data.ItemID, data.sEnrollsProfessionID);
        setTimeout(function () { bindOrderListEvent(orderData); }, 1);
        $('#tabseditFee').tabs('select', 0);
        var givedata = JSON.parse(Easy.bindSelectInfo("../sFee/GetsFeeOrder", data.sFeeID).Data);
        var givedatagrid = $("#editgiveGrid").datagrid("getData");
        for (var i = 0; i < givedata.length; i++) {
            for (var j = 0; j < givedatagrid.rows.length; j++) {
                if (givedata[i].sOrderGiveID === givedatagrid.rows[j].sOrderGiveID) {
                    $("#editgiveGrid").datagrid("selectRow", j);
                }
            }
        }
    }, 1);
};
bindSelectInfomation();

//选择充抵项
bindModfiyOffsetEvent = function (index) {
    totalData = [];
    allData = [];
    $("#ordergird").datagrid("selectRow", index);
    allGrid = "#ordergird";
    allIndex = index;
    var data = $("#ordergird").datagrid("getSelected");
    totalData = JSON.parse(data.OffsetDetail);
    allData = JSON.parse(data.OffsetDetail);
    Easy.OpenDialogEvent("#chooseFeeOffsetItem", "选择充抵项", 780, 590, "../sFee/ChooseFeeOffsetItem", "#chooseFeeOffsetItem-buttons");

}
bindSaveoffsetMethod = function () {
    $("#btnSave_ChooseFeeOffsetItem").click(function () {
        var offsetdata = $("#sfeeOffset").datagrid("getData");
        //if (offsetdata.rows.length === 0) {
        //    Easy.centerShow("系统消息", "没有选择任何充抵，不能保存", 2000);
        //    return false;
        //}
        var tempMoney = 0;
        for (var i = 0; i < offsetdata.rows.length; i++) {
            tempMoney += parseFloat(offsetdata.rows[i].Money);
        }
        var offsetdata = JSON.stringify(offsetdata.rows);
        tempMoney = tempMoney.toFixed("2");
        var row = $("#ordergird").datagrid("getSelected");
        var index = $("#ordergird").datagrid("getRowIndex", row);
        $("#ordergird").datagrid('updateRow', {
            index: index,
            row: {
                OffsetMoney: tempMoney,
                OffsetDetail: offsetdata
            }
        });
        $("#chooseFeeOffsetItem").dialog('close');
        bindSumMoneyEvent("0", "0");
    });
}
bindSaveoffsetMethod();

bindSumMoneyEvent = function (newValue, oldValue) {
    var sum = 0;
    var discount = 0;
    var offsetMoney = 0;

    var data = $("#ordergird").datagrid('getData');
    for (var j = 0; j < data.rows.length; j++) {
        sum += parseFloat(data.rows[j].PaidMoney);
        discount += parseFloat(data.rows[j].DiscountMoney);
        offsetMoney += parseFloat(data.rows[j].OffsetMoney);
    }

    if (oldValue != "") {
        sum = parseFloat(sum) + parseFloat(newValue) - parseFloat(oldValue);
    }
    $("#editShouldMoney").numberspinner("setValue", sum);
    $("#editOffsetMoney").numberspinner("setValue", offsetMoney);
    $("#editDiscountMoney1").numberspinner("setValue", discount);
};

bindSumDiscountMoneyEvent = function (newValue, oldValue) {
    var sum = 0;
    var paidMoney = 0;
    var offsetMoney = 0;

    var data = $("#ordergird").datagrid('getData');
    for (var j = 0; j < data.rows.length; j++) {
        paidMoney += parseFloat(data.rows[j].PaidMoney);
        sum += parseFloat(data.rows[j].DiscountMoney);
        offsetMoney += parseFloat(data.rows[j].OffsetMoney);
    }

    if (oldValue != "") {
        sum = parseFloat(sum) + parseFloat(newValue) - parseFloat(oldValue);
    }

    $("#editShouldMoney").numberspinner("setValue", paidMoney);
    $("#editOffsetMoney").numberspinner("setValue", offsetMoney);
    $("#editDiscountMoney1").numberspinner("setValue", sum);
}