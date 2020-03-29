var sFeeID = "0";//定义ID
var gridText = "";
var deptId = "0";
var sEnrollsProfessionIDs = "0";
var allData = [];
var totalData = [];
var PlanItemID;
firstFunction = function () {
    //加载表格   




    // setTimeout(function () {}

    initTable({ MenuID: menuId })

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //点击添加保存按钮
    bindSaveButtonClickEvent();
    bindSavePrintMethod();
    //修改保存
    bindSaveEditButtonClickEvent();

    bindSaveShoulMoneyEvent();
    bindSaveUplodMethod();
    //选择方案
    bindSaveFeePlanMethod();
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        url: "../sFee/GetsFeeList",
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "sFeeID", checkbox: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "StudName", title: "学生姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "ProName", title: "专业", width: 120, sortable: true },
            { field: "FeeNum", title: "缴费次数", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", sortable: true, align: 'right', halign: 'left' },
            { field: "PaidMoney", title: "实缴金额", sortable: true, align: 'right', halign: 'left' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, align: 'right', halign: 'left' },
            {
                field: "OffsetMoney", title: "充抵金额", sortable: true, align: 'right', halign: 'left',
                formatter: function (value, row, index) {
                    if (row.OffsetString != "" && row.OffsetString != null) {
                        return "<span title=\"" + row.OffsetString.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            {
                field: "ByOffsetMoney", title: "被充抵金额", sortable: true, align: 'right', halign: 'left',
                formatter: function (value, row, index) {
                    if (row.ByOffsetString != "" && row.ByOffsetString != null) {
                        return "<span title=\"" + row.ByOffsetString.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            { field: "RefundMoney", title: "核销金额", sortable: true, align: 'right', halign: 'left' },
            { field: "FeeModel", title: "缴费方式", width: 60, sortable: true },
            { field: "FeeUser", title: "收费人", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 90, sortable: true },
            { field: "Affirm", title: "结账人", width: 80, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 90, sortable: true },
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "EnrollSchool", title: "报名校区", width: 80, sortable: true },
            { field: "Address", title: "地址", width: 150, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        sortName: "sFeeID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
            }
            else if (row.StatusValue === "2") {
                return "color:#339900;";
            }
        },
        //onBeforeLoad: function (param) {
        //    $('#grid').datagrid('reloadFooter', JSON.parse(Easy.bindSelectInfomation("../sFee/GetsFeeListfoot", queryData).Data));
        //},
        onLoadSuccess: function (data) {
            Easy.bindCustomPromptToTableEvent(".tip");
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });

}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
    $("#txtFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });
    $("#txtStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=7",
        valueField: "Value",
        textField: "RefeName",
        panelWidth: 100,
        multiple: true,
        editable: false
    });

    $("#selYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true,
        editable: false
    });

    $("#selMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false,
        panelWidth:120
        
    });

    $("#selPlanSort").combobox({
        url: "../Refe/SelList?RefeTypeID=14",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 200,
        panelWidth: 120,
        multiple: true,
        editable: false
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        sFeeID = "0";
        Easy.OpenDialogEvent("#editsFee", "编辑收费信息", 800, 600, "../sFee/sFeeEdit", "#editsFee-buttons");
    }
}


rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtName: $("#txtName").textbox("getValue"),
        deptId: $("#treeDept").combotree("getValue"),
        idCard: $("#txtIDCard").textbox("getValue"),
        voucherNum: $("#txtVoucherNum").textbox("getValue"),
        noteNum: $("#txtNoteNum").textbox("getValue"),
        status: $("#txtStatus").combobox("getValues").toString(),
        feeMode: $("#txtFeeMode").combobox("getValues").toString(),
        feeTimeS: $("#txtFeeTimeS").datebox("getValue"),
        feeTimeE: $("#txtFeeTimeE").datebox("getValue"),
        txtFeeUser: $("#txtFeeUser").textbox("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        selPlanSort: "" + $("#selPlanSort").combobox("getValues"),
        address: $("#txtAddress").textbox("getValue"),
        enrollSchool: $("#txtEnrollSchool").textbox("getValue"),
        enrollNum:$("#txtEnrollNum").textbox("getValue")
    }
    return queryData;
}

bindSaveButtonClickEvent = function () {
    $("#btnSave").click(function () {
        var feeMode = $("#FeeMode").combobox("getValue");
        if (feeMode === "" || feeMode === null) {
            Easy.centerShow("系统消息", "请选择收费方式", 2000);
            return;
        }
        var isfee = false;
        gridTextTemp = gridText.substring(0, gridText.length - 1);
        if (gridTextTemp === "") {
            return false;
        }
        var gridArr = gridTextTemp.split(',');

        var orderJsonText = "[";
        for (var i = 0; i < gridArr.length; i++) {
            $("#" + gridArr[i]).edatagrid("saveRow");
            var data = $("#" + gridArr[i]).datagrid('getData');
            var numItemId = gridArr[i].replace("feeGrid", '');//获取次数ID
            var dataString = JSON.stringify(data);
            dataString = dataString.substring(1, dataString.length - 1);
            orderJsonText += "{\"NumItemID\":\"" + numItemId + "\"," + dataString + "},"
            if (!isfee) {
                for (var j = 0; j < data.rows.length; j++) {
                    var paidMoney = data.rows[j].PaidMoney;
                    var discountMoney = data.rows[j].DiscountMoney;
                    var offsetMoney = data.rows[j].OffsetMoney;
                    if (parseFloat(paidMoney) > 0 || parseFloat(discountMoney) > 0 || parseFloat(offsetMoney) > 0) {
                        isfee = true;
                        break;
                    }
                }
            }
        }

        //验证金额是否正确
        var flag = false;
        var errorMessage = "";//错误提示信息
        for (var i = 0; i < gridArr.length; i++) {
            $("#" + gridArr[i]).edatagrid("saveRow");
            var data = $("#" + gridArr[i]).datagrid('getData');
            for (var j = 0; j < data.rows.length; j++) {
                var tempMoney = parseFloat(data.rows[j].PaidMoney) + parseFloat(data.rows[j].DiscountMoney) + parseFloat(data.rows[j].OffsetMoney) + parseFloat(data.rows[j].ReceivedMoney);
                if (tempMoney > parseFloat(data.rows[j].ShouldMoney)) {
                    flag = true;
                    errorMessage = "" + data.rows[j].NumName + "" + data.rows[j].DetailName + ": 实收+充抵+优惠+已收 不能大于应收金额 " + data.rows[j].ShouldMoney + " 元";
                    break;
                }
            }
            if (flag) {
                break;
            }
        }
        if (flag) {
            Easy.centerShow("系统消息", errorMessage, 2000);
            return false;
        }
        var vali = false;
        var giveData = $("#giveGrid").datagrid('getSelections');
        var numItemId1 = $("#selNumItemID").combobox("getValue");
        if (giveData.length > 0) {
            if (numItemId1 === "" || numItemId1 === null)
                vali = true;
        }

        if (vali) {
            Easy.centerShow("系统消息", "请选择配品归属的缴费次数", 2000);
            return false;
        }
        vali = false;
        if (numItemId1 != "" && numItemId1 != null) {
            if (giveData.length === 0) {
                vali = true;
            }
        }
        if (vali) {
            Easy.centerShow("系统消息", "请勾选配品", 2000);
            return false;
        }
        orderJsonText = orderJsonText.substring(0, orderJsonText.length - 1);
        orderJsonText += "]";
        $("#OrderJsonData").textbox("setValue", orderJsonText);
        var giveData = $("#giveGrid").datagrid('getSelections');//获取配品列表
        $("#GiveJsonData").textbox("setValue", JSON.stringify(giveData));

        if (!isfee) {
            Easy.centerShow("系统消息", "没有收取任何费用", 3000);
            return;
        }
        if ($("#btnSave").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
                $("#fSave").form("submit", {
                    url: "../sFee/GetsFeeEdit",
                    onSubmit: function () {
                        var validate = $("#fSave").form("validate");//验证
                        if (validate) {
                            $("#btnSave").linkbutton("disable");//禁用按钮
                            $("#btnSaveProduceOrder").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        result = JSON.parse(data);
                        if (result.IsError === false) {
                            Easy.centerShow("系统消息", result.Message, 3000);
                            $("#editsFee").dialog("close");//关闭弹窗
                            $("#grid").datagrid("load");//刷新表格
                        }
                        else
                            Easy.centerShow("系统消息", result.Message, 3000);

                        $("#btnSave").linkbutton("enable");//解除按钮禁用
                        $("#btnSaveProduceOrder").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
}


edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费信息")) {
            sFeeID = rows[0].sFeeID;
            Easy.OpenDialogEvent("#modifysFee", "编辑收费信息", 800, 600, "../sFee/sFeeModify", "#modifysFee-buttons");
        }
    }
};
//打印
print = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "print")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRows(rows, "收费信息")) {
            $.messager.confirm("", "确定要打印吗？", function (yes) {
                if (yes) {
                    var sfeeString = "";
                    var voucher = "";
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i].EnrollStatus === "2") {
                            voucher = rows[i].VoucherNum;
                            break;
                        }
                        sfeeString += rows[i].sFeeID + ",";;
                    }
                    if (voucher != "") {
                        Easy.centerShow("系统消息", "" + voucher + "是预报名,不能打印明细", 3000);
                        return false;
                    }
                    sfeeString = sfeeString.substring(0, sfeeString.length - 1);
                    var result = Easy.bindSelectInfo("../sFee/GetPrintInfo", sfeeString);
                    if (result.Message === "success") {
                        var data = JSON.parse(result.Data);
                        var LODOP; //声明为全局变量
                        LODOP = getLodop();
                        for (var j = 0; j < data.length; j++) {
                            var noteNum = "";
                            $.ajax({
                                type: "post",
                                url: "../sFee/UpdateNoteNum",
                                async: false,
                                data: { sFeeID: data[j].sFeeID },
                                dataType: "json",
                                success: function (result) {

                                    if (result.IsError === false) {
                                        $("#grid").datagrid("load");
                                        LODOP.PRINT_INIT("学费收据");
                                        LODOP.SET_PRINTER_INDEX(-1);
                                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                                        var html = Easy.AddStuFeePrintContent(data[j], result.Data);
                                        LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                                        LODOP.PRINT();
                                        //  LODOP.PREVIEW();
                                    }
                                },
                                error: function () {
                                    Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                                }

                            });
                            Easy.centerShow("系统消息", "打印成功", 3000);
                        }
                    }
                    else
                        Easy.centerShow("系统消息", result.Message, 2000);
                }
            });
        }
    }
};
//重新打印
reprint = function () {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    if (Easy.checkRows(rows, "收费信息")) {
        $.messager.confirm("", "确定要重新打印吗？", function (yes) {
            if (yes) {
                var sfeeString = "";
                var voucher = "";
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].EnrollStatus === "2") {
                        voucher = rows[i].VoucherNum;
                        break;
                    }
                    sfeeString += rows[i].sFeeID + ",";;
                }
                if (voucher != "") {
                    Easy.centerShow("系统消息", "" + voucher + "是预报名,不能打印明细", 3000);
                    return false;
                }
                sfeeString = sfeeString.substring(0, sfeeString.length - 1);
                var result = Easy.bindSelectInfo("../sFee/GetRePrintInfo", sfeeString);
                if (result.Message === "success") {
                    var data = JSON.parse(result.Data);
                    var LODOP; //声明为全局变量
                    LODOP = getLodop();
                    for (var j = 0; j < data.length; j++) {
                        var noteNum = "";
                        $.ajax({
                            type: "post",
                            url: "../sFee/UpdateNoteNum",
                            async: false,
                            data: { sFeeID: data[j].sFeeID },
                            dataType: "json",
                            success: function (result) {
                                if (result.IsError === false) {
                                    $("#grid").datagrid("load");
                                    LODOP.PRINT_INIT("学费收据");
                                    LODOP.SET_PRINTER_INDEX(-1);
                                    LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                                    var html = Easy.AddStuFeePrintContent(data[j], result.Data);
                                    LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                                    // LODOP.PRINT();
                                    LODOP.PREVIEW();
                                }
                            },
                            error: function () {
                                Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                            }
                        });
                        Easy.centerShow("系统消息", "重新打印成功", 2000);
                    }

                }
                else
                    Easy.centerShow("系统消息", result.Message, 2000);
            }
        });
    }
};
//打印合计
printTotal = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "printTotal")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRows(rows, "收费信息")) {
            $.messager.confirm("", "确定要打印吗？", function (yes) {
                if (yes) {
                    var sfeeString = "";
                    for (var i = 0; i < rows.length; i++) {
                        sfeeString += rows[i].sFeeID + ",";;
                    }
                    sfeeString = sfeeString.substring(0, sfeeString.length - 1);
                    var result = Easy.bindSelectInfo("../sFee/GetPrintInfo", sfeeString);
                    if (result.Message === "success") {
                        var data = JSON.parse(result.Data);
                        var LODOP; //声明为全局变量
                        LODOP = getLodop();
                        for (var j = 0; j < data.length; j++) {
                            var noteNum = "";
                            $.ajax({
                                type: "post",
                                url: "../sFee/UpdateNoteNum",
                                async: false,
                                data: { sFeeID: data[j].sFeeID },
                                dataType: "json",
                                success: function (result) {
                                    if (result.IsError === false) {
                                        $("#grid").datagrid("load");
                                        LODOP.PRINT_INIT("学费收据");
                                        LODOP.SET_PRINTER_INDEX(-1);
                                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                                        var html = Easy.AddStuFeePrintTotalContent(data[j], result.Data);
                                        LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                                        LODOP.PRINT();
                                        // LODOP.PREVIEW();
                                    }
                                },
                                error: function () {
                                    Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                                }

                            });
                            Easy.centerShow("系统消息", "打印成功", 3000);
                        }
                    }
                    else
                        Easy.centerShow("系统消息", result.Message, 2000);
                }
            });
        }
    }
};
//结账
affirm = function () {
    var data = $("#grid").datagrid("getSelections");
    if (data.length === 0) {
        Easy.centerShow("系统消息", "请选择收费信息", 2000);
        return false;
    }
    var flag = false;
    for (var i = 0; i < data.length; i++) {
        if (data[i].StatusValue === "2") {
            flag = true;
            break;
        }
    }
    if (flag) {
        Easy.centerShow("系统消息", "" + data[i].VoucherNum + "已结账", 2000);
        return false;
    }
    var IdArray = bindGetRowID(data);
    bindUpdateAffirmMethod("结账", "2", IdArray);
};
//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费信息")) {
            sFeeID = rows[0].sFeeID;
            Easy.OpenDialogEvent("#viewsFee", "查看缴费详情", 800, 600, "../sFee/sFeeView", "#viewsFee-buttons");
        }
    }
};
//反结账
returnaffirm = function () {
    var data = $("#grid").datagrid("getSelections");
    if (data.length === 0) {
        Easy.centerShow("系统消息", "请选择收费信息", 2000);
        return false;
    }
    var IdArray = bindGetRowID(data);
    bindUpdateAffirmMethod("反结账", "1", IdArray);
};
//作废
disable = function () {
    var data = $("#grid").datagrid("getSelections");
    if (data.length === 0) {
        Easy.centerShow("系统消息", "请选择收费信息", 2000);
        return false;
    }

    var IdArray = bindGetRowID(data);
    bindUpdateStatusMethod("作废", "9", IdArray);
};

//重置打印次数
reset = function () {
    var data = $("#grid").datagrid("getSelections");
    if (data.length === 0) {
        Easy.centerShow("系统消息", "请选择收费信息", 2000);
        return false;
    }
    $.messager.confirm("", "确定要重置么？", function (yes) {
        if (yes) {
            var IdArray = bindGetRowID(data);
            var result = Easy.bindSelectInfo("../sFee/GetsFeeResetPrintNum", IdArray);
            if (result.Message = "success")
                Easy.centerShow("系统消息", "重置成功", 2000);
            else
                Easy.centerShow("系统消息", result.Message, 2000);
        }
    });
};
//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../sFee/DownloadsFee",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}
//获取id数组
bindGetRowID = function (data) {
    var id = "";
    for (var i = 0; i < data.length; i++) {
        id += data[i].sFeeID + ",";
    }
    return id.substring(0, id.length - 1);
}


bindUpdateStatusMethod = function (confirmstr, status, id) {
    Easy.bindUpdateValue(confirmstr, status, "../sFee/GetUpdatesFeeStatus", id, "1", "#grid");
}
bindUpdateAffirmMethod = function (confirmstr, status, id) {
    Easy.bindUpdateValue(confirmstr, status, "../sFee/UpdatesFeeAffirm", id, "1", "#grid");
}

bindSaveEditButtonClickEvent = function () {
    $("#btnSave_modifysFee").click(function () {
        var giveData = $("#editgiveGrid").datagrid('getSelections');//获取配品列表
        var orderData = $("#ordergird").datagrid("getData");
        $("#editGiveJsonData").textbox("setValue", JSON.stringify(giveData));
        $("#editOrderJsonData").textbox("setValue", JSON.stringify(orderData));
        if ($("#btnSave_modifysFee").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
                $("#feditSave").form("submit", {
                    url: "../sFee/GetMondfiysFee",
                    onSubmit: function () {
                        var validate = $("#feditSave").form("validate");//验证
                        if (validate) {
                            $("#btnSave_modifysFee").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        result = JSON.parse(data);
                        if (result.IsError === false) {
                            if (result.Message === "success") {
                                Easy.centerShow("系统消息", result.Data, 3000);
                                $("#modifysFee").dialog("close");//关闭弹窗
                                $("#grid").datagrid("load");//刷新表格
                            }
                            else {
                                Easy.centerShow("系统消息", result.Message, 3000);
                            }
                        }
                        else
                            Easy.centerShow("系统消息", result.Message, 3000);

                        $("#btnSave_modifysFee").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
}

//绑定保存修改应收金额点击事件
bindSaveShoulMoneyEvent = function () {
    $("#btnSave_changeShouldMoney").click(function () {
        bindSubmitChangeShouldMoneyMethod();
    });

}
//修改应收金额
bindSubmitChangeShouldMoneyMethod = function () {
    $("#fSearch_ChangeShouldMoney").form("submit", {
        url: "../sOrderNote/GetsOrderNoteEdit",
        onSubmit: function () {
            var validate = $("#fSearch_ChangeShouldMoney").form("validate");//验证
            if (validate) {
                $("#btnSave_changeShouldMoney").linkbutton("disable");//禁用按钮
            }
            return validate;
        },
        success: function (result) {
            var Data = JSON.parse(result);
            setTimeout(function () {
                $("#btnSave_changeShouldMoney").linkbutton("enable");
            }, 1000);
            if (Data.Message === "success") {
                var shouldMoney = $("#ChangeShouldMoney").numberspinner("getValue");
                var UnPaid = parseFloat(shouldMoney) - parseFloat(receivedMoney);
                $(selectGrid).datagrid('updateRow', {
                    index: selectiIndex,
                    row: {
                        ShouldMoney: shouldMoney,
                        UnPaid: UnPaid.toFixed(2).toString()
                    }
                });
                var shouldMoneySum = 0;
                var UnPaidSum = 0;
                var data = $(selectGrid).datagrid('getData');
                for (var i = 0; i < data.rows.length; i++) {
                    shouldMoneySum += parseFloat(data.rows[i].ShouldMoney);
                    UnPaidSum += parseFloat(data.rows[i].UnPaid);
                }
                var rows = $(selectGrid).datagrid('getFooterRows');
                rows[0]['ShouldMoney'] = '合计:' + shouldMoneySum.toFixed(2).toString();
                rows[0]['UnPaid'] = '合计:' + UnPaidSum.toFixed(2).toString();
                $(selectGrid).datagrid('reloadFooter');
                Easy.centerShow("系统消息", "修改成功", 1000);
                $("#changeShouldMoney").dialog('close');
            }
            else {
                Easy.centerShow("系统消息", Data.Data, 2000);
            }
        }
    });
};

//重新打印合计
reprintTatal = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "reprintTatal")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRows(rows, "收费信息")) {
            $.messager.confirm("", "确定要打印吗？", function (yes) {
                if (yes) {
                    var sfeeString = "";
                    for (var i = 0; i < rows.length; i++) {
                        sfeeString += rows[i].sFeeID + ",";;
                    }
                    sfeeString = sfeeString.substring(0, sfeeString.length - 1);
                    var result = Easy.bindSelectInfo("../sFee/GetPrintInfoNoPrintNum", sfeeString);
                    var data = JSON.parse(result.Data);
                    var LODOP; //声明为全局变量
                    LODOP = getLodop();
                    for (var j = 0; j < data.length; j++) {
                        var noteNum = "";
                        $.ajax({
                            type: "post",
                            url: "../sFee/UpdateNoteNum",
                            async: false,
                            data: { sFeeID: data[j].sFeeID },
                            dataType: "json",
                            success: function (result) {
                                if (result.IsError === false) {
                                    $("#grid").datagrid("load");
                                    LODOP.PRINT_INIT("学费收据");
                                    LODOP.SET_PRINTER_INDEX(-1);
                                    LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                                    var html = Easy.AddStuFeePrintTotalContent(data[j], result.Data);
                                    LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                                    // LODOP.PRINT();
                                    LODOP.PREVIEW();
                                }
                            },
                            error: function () {
                                Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                            }

                        });
                        Easy.centerShow("系统消息", "打印成功", 3000);
                    }
                }
            });
        }
    }
}

bindSavePrintMethod = function () {

    $("#btnSavePrintDetail").click(function () {
        var feeMode = $("#FeeMode").combobox("getValue");
        if (feeMode === "" || feeMode === null) {
            Easy.centerShow("系统消息", "请选择收费方式", 2000);
            return;
        }
        bindSaveProduceOrderMethod("1", "#btnSavePrintDetail", "#btnSavePrintTotal");
    });
    $("#btnSavePrintTotal").click(function () {
        var feeMode = $("#FeeMode").combobox("getValue");
        if (feeMode === "" || feeMode === null) {
            Easy.centerShow("系统消息", "请选择收费方式", 2000);
            return;
        }
        bindSaveProduceOrderMethod("2", "#btnSavePrintTotal", "#btnSavePrintDetail");
    });
}

bindSaveProduceOrderMethod = function (val, btn, disbtn) {
    var isfee = false;
    gridTextTemp = gridText.substring(0, gridText.length - 1);
    if (gridTextTemp === "") {
        return false;
    }
    var gridArr = gridTextTemp.split(',');

    var orderJsonText = "[";
    for (var i = 0; i < gridArr.length; i++) {
        $("#" + gridArr[i]).edatagrid("saveRow");
        var data = $("#" + gridArr[i]).datagrid('getData');
        var numItemId = gridArr[i].replace("feeGrid", '');//获取次数ID
        var dataString = JSON.stringify(data);
        dataString = dataString.substring(1, dataString.length - 1);
        orderJsonText += "{\"NumItemID\":\"" + numItemId + "\"," + dataString + "},"
        if (!isfee) {
            for (var j = 0; j < data.rows.length; j++) {
                var paidMoney = data.rows[j].PaidMoney;
                var discountMoney = data.rows[j].DiscountMoney;
                var offsetMoney = data.rows[j].OffsetMoney;
                if (parseFloat(paidMoney) > 0 || parseFloat(discountMoney) > 0 || parseFloat(offsetMoney) > 0) {
                    isfee = true;
                    break;
                }
            }
        }
    }

    //验证金额是否正确
    var flag = false;
    var errorMessage = "";//错误提示信息
    for (var i = 0; i < gridArr.length; i++) {
        $("#" + gridArr[i]).edatagrid("saveRow");
        var data = $("#" + gridArr[i]).datagrid('getData');
        for (var j = 0; j < data.rows.length; j++) {
            var tempMoney = parseFloat(data.rows[j].PaidMoney) + parseFloat(data.rows[j].DiscountMoney) + parseFloat(data.rows[j].OffsetMoney) + parseFloat(data.rows[j].ReceivedMoney);
            if (tempMoney > parseFloat(data.rows[j].ShouldMoney)) {
                flag = true;
                errorMessage = "" + data.rows[j].NumName + "" + data.rows[j].DetailName + ": 实收+充抵+优惠+已收 不能大于应收金额 " + data.rows[j].ShouldMoney + " 元";
                break;
            }
        }
        if (flag) {
            break;
        }
    }
    if (flag) {
        Easy.centerShow("系统消息", errorMessage, 2000);
        return false;
    }
    var vali = false;
    var giveData = $("#giveGrid").datagrid('getSelections');
    var numItemId1 = $("#selNumItemID").combobox("getValue");
    if (giveData.length > 0) {
        if (numItemId1 === "" || numItemId1 === null)
            vali = true;
    }

    if (vali) {
        Easy.centerShow("系统消息", "请选择缴费次数", 2000);
        return false;
    }
    vali = false;
    if (numItemId1 != "" && numItemId1 != null) {
        if (giveData.length === 0) {
            vali = true;
        }
    }
    if (vali) {
        Easy.centerShow("系统消息", "请选择勾选配品", 2000);
        return false;
    }
    orderJsonText = orderJsonText.substring(0, orderJsonText.length - 1);
    orderJsonText += "]";
    $("#OrderJsonData").textbox("setValue", orderJsonText);
    var giveData = $("#giveGrid").datagrid('getSelections');//获取配品列表
    $("#GiveJsonData").textbox("setValue", JSON.stringify(giveData));

    if (!isfee) {
        Easy.centerShow("系统消息", "没有收取任何费用", 3000);
        return;
    }
    if ($(btn).linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
        if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
            $("#fSave").form("submit", {
                url: "../sFee/GetsFeeEdit",
                onSubmit: function () {
                    var validate = $("#fSave").form("validate");//验证
                    if (validate) {
                        $("#btnSave").linkbutton("disable");//禁用按钮
                        $(btn).linkbutton("disable");//禁用按钮
                        $(disbtn).linkbutton("disable");//禁用按钮
                    }
                    return validate;
                },
                success: function (data) {
                    result = JSON.parse(data);
                    if (result.IsError === false) {
                        var tempDetpId = $("#DeptID").combotree("getValue");
                        var tempTime = $("#FeeTime").datebox("getValue");
                        $("#fSave").form("reset");
                        $("#DeptID").combotree("setValue", tempDetpId);
                        $("#FeeTime").datebox("setValue", tempTime);
                        $('#ExamNum').textbox('textbox').focus();
                        closeAll();
                        var tempData = JSON.parse(result.Data);
                        var sfeeString = "";
                        if (tempData.length === 1) {
                            for (var i = 0; i < tempData.length; i++) {
                                var result1 = Easy.bindSelectInfo("../sFee/GetPrintInfo", tempData[i].sFeeId);
                                if (result1.IsError === true) {
                                    Easy.centerShow("系统消息", result1.Message, 2000);
                                    return false;
                                }
                                var printData = JSON.parse(result1.Data);
                                var LODOP; //声明为全局变量
                                LODOP = getLodop();
                                var noteNum = "";
                                $.ajax({
                                    type: "post",
                                    url: "../sFee/UpdateNoteNum",
                                    async: false,
                                    data: { sFeeID: tempData[i].sFeeId },
                                    dataType: "json",
                                    success: function (result2) {
                                        if (result2.IsError === false) {
                                            LODOP.PRINT_INIT("学费收据");
                                            LODOP.SET_PRINTER_INDEX(-1);
                                            LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                                            var html = "";
                                            if (val === "2") {
                                                html = Easy.AddStuFeePrintTotalContent(printData[0], result2.Data);
                                            }
                                            else {
                                                html = Easy.AddStuFeePrintContent(printData[0], result2.Data);
                                            }
                                            LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                                            LODOP.PRINT();
                                            //LODOP.PREVIEW();
                                        }

                                    },
                                    error: function () {
                                        Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                                    }

                                });
                            }
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message + "，但未打印，请手工打印", 3000);
                        }
                        $("#grid").datagrid("load");//刷新表格
                    }
                    else {
                        Easy.centerShow("系统消息", result.Message, 3000);
                    }
                    $("#btnSave").linkbutton("enable");//解除按钮禁用
                    $(btn).linkbutton("enable");//解除按钮禁用
                    $(disbtn).linkbutton("enable");//解除按钮禁用
                }


            });
        }
    }
}

upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#uploadsFee", "导入学费收费信息", 800, 600, "../sFee/sFeeUpload", "#uploadsFee-buttons");
    }
}

deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/学费批量收取模板.xls";
    }
}

bindSaveUplodMethod = function () {
    $("#btnSaveUploadsFee").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "学费收费信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload", "../sFee/GetsFeeUpload", "#btnSaveUploadsFee", "#grid", "1", "upload", "#gridStudent", data.rows, "#stuLayout");
    });
}


deletesfee = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deletesfee")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费信息")) {
            $.messager.confirm("", "确定要删除吗？", function (yes) {
                if (yes) {
                    var result = Easy.bindSelectInfomation("../sFee/DeletesFee", { sfeeId: rows[0].sFeeID });
                    if (result.IsError === false) {
                        $("#grid").datagrid('reload');
                        Easy.centerShow("系统消息", result.Message, 2000);

                    }
                    else {
                        Easy.centerShow("系统消息", result.Message, 2000);
                    }
                }
            });
        }
    }
};

newprint = function () {

    if (Easy.bindPowerValidationEvent(menuId, "1", "print")) {
        bindPrintDetailMethod(false, false);
    }
};
newreprint = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "newreprint")) {
        bindPrintDetailMethod(true, false);
    }
};
newprintTotal = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "newprintTotal")) {
        bindPrintDetailMethod(false, true);
    }
};
newreprintTatal = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "newreprintTatal")) {

        bindPrintDetailMethod(true, true);
    }
};

bindPrintDetailMethod = function (isReprint, isTotal) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    if (Easy.checkRows(rows, "收费信息")) {
        $.messager.confirm("", "确定要打印吗？", function (yes) {
            if (yes) {
                var sfeeString = "";
                var voucher = "";
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].EnrollStatus === "2") {
                        voucher = rows[i].VoucherNum;
                        break;
                    }
                    sfeeString += rows[i].sFeeID + ",";;
                }
                if (voucher != "") {
                    Easy.centerShow("系统消息", "" + voucher + "是预报名,不能打印明细", 3000);
                    return false;
                }
                sfeeString = sfeeString.substring(0, sfeeString.length - 1);
                var result = Easy.bindSelectInfomation("../sFee/GetPrintInfoNew", { sfeeId: sfeeString, isReprint: isReprint });
                if (result.Message === "success") {
                    var data = JSON.parse(result.Data);
                    var LODOP; //声明为全局变量
                    LODOP = getLodop();
                    for (var j = 0; j < data.length; j++) {
                        var noteNum = "";
                        $.ajax({
                            type: "post",
                            url: "../sFee/UpdateNoteNum",
                            async: false,
                            data: { sFeeID: data[j].sFeeID },
                            dataType: "json",
                            success: function (result) {

                                if (result.IsError === false) {
                                    $("#grid").datagrid("load");
                                    LODOP.PRINT_INIT("学费收据");
                                    LODOP.SET_PRINTER_INDEX(-1);
                                    LODOP.SET_PRINT_PAGESIZE(1, 1400, 2000, "");
                                    if (isTotal) {
                                        var html = Easy.PrintsFeeTotalNew(data[j], result.Data);
                                        LODOP.ADD_PRINT_HTM(0, 0, 530, 750, html);
                                        LODOP.PREVIEW();
                                    }
                                    else {
                                        var html = Easy.PrintsFeeDetatilNew(data[j], result.Data);
                                        LODOP.ADD_PRINT_HTM(0, 0, 530, 750, html);
                                        LODOP.PREVIEW();
                                    }
                                }
                            },
                            error: function () {
                                Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                            }

                        });
                        Easy.centerShow("系统消息", "打印成功", 3000);
                    }
                }
                else
                    Easy.centerShow("系统消息", result.Message, 2000);
            }
        });
    }
}



//方案
bindSaveFeePlanMethod = function () {
    $("#btnSavesFeePlan").click(function () {
        var rows = $("#chooseFeePlanGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "方案信息")) {
            $("#sEnrollID").textbox("setValue", rows[0].sEnrollID);
            $("#sEnrollsProfessionID").textbox("setValue", rows[0].sEnrollsProfessionID);
            $("#sEnrollText").textbox("setValue", rows[0].StudentName + '_' + rows[0].ProName + '_' + rows[0].PlanName);
            $("#choosesFeePlan").dialog('close');
            PlanItemID = rows[0].ItemID;
            closeAll();//关闭所有标签   
            isSelect = false;//初始化
            bindOrderEvent(rows[0].ItemID, rows[0].sEnrollsProfessionID);//加载订单tabs
            bindGiveEvent(rows[0].ItemID, rows[0].sEnrollsProfessionID);//加载配品tabs
            setTimeout(function () {
                bindLoadsOrderCombobox(rows[0].ItemID, rows[0].sEnrollsProfessionID);//加载缴费次数Combobox
                bindDiscountCombobox(rows[0].ItemID);
            }, 1);

            $('#tabsFee').tabs('select', 0);//设置选中第几个选项卡
        }
    });
};

