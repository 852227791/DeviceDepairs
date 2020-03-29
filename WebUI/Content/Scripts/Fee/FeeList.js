var FeeID = "0";//定义ID
var DeptID = "0 ";
var itemId = "0";
var tempData = null;
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);
    bindSaveUplodMethod();
    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editFee", "#fSave", "../Fee/GetFeeEdit", "#btnSave", "1", "#grid", menuId, "1", "#editFeeID");
    //  Easy.bindSaveUploadFile("#upFee", "#fUpload", "../Fee/UpLoadBFee", "#btnSaveupFee", "1", "#grid", menuId, "1", "upload");
    bindPrintMore();
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "FeeID", checkbox: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "Dept", title: "校区", width: 140, sortable: true },
            { field: "Name", title: "学生姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "EnrollNum", title: "学号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "ItemName", title: "证书名称", width: 100, sortable: true },
            { field: "FeeContent", title: "收费项目", width: 140, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "FeeMode", title: "交费方式", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实交金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            {
                field: "OffsetMoney", title: "冲抵金额", sortable: true, halign: 'left', align: 'right',
                formatter: function (value, row, index) {
                    if (row.OffsetString != "" && row.OffsetString != null) {
                        var str = "<span title=\"" + row.OffsetString.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                        return str;
                    }
                    else {
                        return value;
                    }
                }
            },
            {
                field: "BeOffsetMoney", title: "被冲抵金额", sortable: true, halign: 'left', align: 'right',
                formatter: function (value, row, index) {
                    if (row.BeOffsetString != "" && row.BeOffsetString != null) {
                        return "<span title=\"" + row.BeOffsetString.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "CreateName", title: "收费人", width: 60, sortable: true },
            { field: "ProName", title: "专业", width: 140, sortable: true },
            { field: "ClassName", title: "班级", width: 120, sortable: true },
            { field: "PersonSort", title: "交款人员", width: 60, sortable: true },
            { field: "Teacher", title: "教师", width: 60, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "Affirm", title: "结账人", width: 60, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 80, sortable: true }
        ]],
        url: "../Fee/GetFeeList", sortName: "FeeID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
            }
            else if (row.StatusValue === "2") {
                return "color:#339900;";
            }
        },
        onLoadSuccess: function (data) {
            Easy.bindCustomPromptToTableEvent(".tip");
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
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
    $("#selFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=7",
        valueField: "Value",
        textField: "RefeName",
        panelWidth: 100,
        multiple: true,
        editable: false
    });
    $("#txtFeeTimeS").datebox({});
    $("#txtFeeTimeE").datebox({});
    $("#selPersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        FeeID = "0";
        Easy.OpenDialogEvent("#editFee", "编辑收费信息", 680, 550, "../Fee/FeeEdit", "#editFee-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费信息")) {
            FeeID = rows[0].FeeID;
            Easy.OpenDialogEvent("#editFee", "编辑收费信息", 680, 550, "../Fee/FeeEdit", "#editFee-buttons");
        }
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费信息")) {
            FeeID = rows[0].FeeID;
            Easy.OpenDialogEvent("#viewFee", "查看收费信息", 800, 600, "../Fee/FeeView", "#viewFee-buttons");
        }
    }
}

deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/证书收费导入模板.xls";
    }
}

//结账
affirm = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "affirm")) {
        var rows = $("#grid").datagrid("getSelections");
        var flag = false;
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].StatusValue === "2") {
                flag = true;
                break;
            }
        }
        if (flag) {
            Easy.centerShow("系统消息", "" + rows[i].VoucherNum + "已结账", 2000);
            return false;
        }
        bindUpdateStatus("结账", "2");
       
    }
}
//反结账
returnaffirm = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "returnaffirm")) {
        bindUpdateStatus("反结账", "1");
    }
}
//作废
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("作废", "9");
    }
}
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRows(rows, "收费信息")) {
        var rowsIdStr = "";
        for (var i = 0; i < rows.length; i++) {
            rowsIdStr += rows[i].FeeID + ",";
        }
        if (rowsIdStr != "") {
            rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
        }
        //批量修改状态
        Easy.bindUpdateValues(confirmstr, status, "../Fee/GetUpdatesStatus", rowsIdStr, "1", "#grid");
    }
}
bindResetPrintNum = function (confirmstr, printNum) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRows(rows, "收费信息")) {
        var rowsIdStr = "";
        for (var i = 0; i < rows.length; i++) {
            rowsIdStr += rows[i].FeeID + ",";
        }
        if (rowsIdStr != "") {
            rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
        }
        //重置打印次数
        Easy.bindUpdateValues(confirmstr, printNum, "../Fee/GetResetPrintNum", rowsIdStr, "1", "#grid");
    }
}
print = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "print")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRows(rows, "收费信息")) {
            $.messager.confirm("", "确定要打印吗？", function (c) {
                if (c) {
                    var data = { "jsontext": [] };
                    var isError = rows.length;
                    var errorStr = "";
                    for (var i = 0; i < rows.length; i++) {
                        var result = Easy.bindSelectInfo("../Fee/GetPrintInfo", rows[i].FeeID);
                        if (result.IsError === false) {
                            isError -= 1;
                            var row = {};
                            row.result = result.Message;
                            data.jsontext.push(row);
                        }
                        else {
                            errorStr += result.Message + "\n";
                        }
                    }

                    if (isError === 0) {
                        var LODOP; //声明为全局变量
                        LODOP = getLodop();
                        for (var j = 0; j < rows.length; j++) {
                            var model = JSON.parse(data.jsontext[j].result)[0];
                            var noteNum = "";
                            $.ajax({
                                type: "post",
                                url: "../Fee/UpdateNoteNum",
                                async: false,
                                data: { FeeID: model.FeeID },
                                dataType: "json",
                                success: function (result) {
                                    if (result.IsError === false) {
                                        $("#grid").datagrid("load");
                                        noteNum = result.Message;

                                        LODOP.PRINT_INIT("证书费收据");
                                        LODOP.SET_PRINTER_INDEX(-1);
                                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 1000, "");
                                        var html = Easy.AddProvePrintContent(model.ParentDeptName, model.VoucherNum, noteNum, model.DeptName, model.StudentName, model.IDCard, model.FeeMode, model.FeeTime, model.ItemName, model.ShouldMoney, model.FeeContent, model.Remark1 + model.Remark2 + model.Remark3 + model.Remark4 + model.Remark5 + model.Remark, model.ClassText, model.Teacher, model.Feeer);
                                        LODOP.ADD_PRINT_HTM(0, 0, 450, 370, html);
                                        LODOP.PRINT();
                                        //LODOP.PREVIEW();
                                    }
                                },
                                error: function () {
                                    Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                                }
                            });
                        }
                    }
                    else {
                        Easy.centerShow("系统消息", errorStr, 3000);
                    }
                }
            });
        }
    }
}
reprint = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "reprint")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "收费信息")) {
            $.messager.confirm("", "确定要打印吗？", function (c) {
                if (c) {
                    var data = { "jsontext": [] };
                    var isError = rows.length;
                    var errorStr = "";
                    for (var i = 0; i < rows.length; i++) {
                        var result = Easy.bindSelectInfo("../Fee/GetPrintInfoNoPrintNum", rows[i].FeeID);
                        if (result.IsError === false) {
                            isError -= 1;
                            var row = {};
                            row.result = result.Message;
                            data.jsontext.push(row);
                        }
                        else {
                            errorStr += result.Message + "\n";
                        }
                    }
                    if (isError === 0) {
                        var LODOP; //声明为全局变量
                        LODOP = getLodop();
                        for (var j = 0; j < rows.length; j++) {
                            var model = JSON.parse(data.jsontext[j].result)[0];
                            var noteNum = "";
                            $.ajax({
                                type: "post",
                                url: "../Fee/UpdateNoteNum",
                                async: false,
                                data: { FeeID: model.FeeID },
                                dataType: "json",
                                success: function (result) {
                                    if (result.IsError === false) {
                                        $("#grid").datagrid("load");
                                        noteNum = result.Message;

                                        LODOP.PRINT_INIT("证书费收据");
                                        LODOP.SET_PRINTER_INDEX(-1);
                                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 1000, "");
                                        var html = Easy.AddProvePrintContent(model.ParentDeptName, model.VoucherNum, noteNum, model.DeptName, model.StudentName, model.IDCard, model.FeeMode, model.FeeTime, model.ItemName, model.ShouldMoney, model.FeeContent, model.Remark1 + model.Remark2 + model.Remark3 + model.Remark4 + model.Remark5 + model.Remark, model.ClassText, model.Teacher, model.Feeer);
                                        LODOP.ADD_PRINT_HTM(0, 0, 450, 370, html);
                                        //LODOP.PRINT();
                                        LODOP.PREVIEW();
                                    }
                                },
                                error: function () {
                                    Easy.centerShow("系统消息", "打印出错，请联系管理员", 3000);
                                }
                            });
                        }
                    }
                    else {
                        Easy.centerShow("系统消息", errorStr, 3000);
                    }
                }
            });
        }
    }
}
reset = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "reset")) {
        bindResetPrintNum("重置", "0");
    }
}
rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtName: $("#txtName").val(),
        txtIDCard: $("#txtIDCard").val(),
        txtVoucherNum: $("#txtVoucherNum").val(),
        txtNoteNum: $("#txtNoteNum").val(),
        txtCreateName: $("#txtCreateName").val(),
        treeDept: $("#treeDept").combotree("getValue"),
        selFeeMode: "" + $("#selFeeMode").combobox("getValues"),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        txtFeeTimeS: $("#txtFeeTimeS").datebox("getValue"),
        txtFeeTimeE: $("#txtFeeTimeE").datebox("getValue"),
        txtProveName: $("#txtProveName").val(),
        selPersonSort: "" + $("#selPersonSort").combobox("getValues")
    }
    return queryData;
}
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../Fee/DownloadFee",
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
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#upFee", "导入收费信息", 800, 600, "../Fee/FeeUp", "#upFee-buttons");
    }
}

bindSaveUplodMethod = function () {
    $("#btnSaveupFee").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有交费人信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload", "../Fee/GetUpdateProveFee", "#btnSaveupFee", "#grid", "1", "upload", "#gridStudent", data.rows, "#feeLayout");
    });
}


printMore = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "printMore")) {
        Easy.OpenDialogEvent("#printFee", "合并打印证书收费", 800, 600, "../Fee/PrintFee", "#printFee-buttons");
    }
}
bindPrintMore = function () {
    $("#btnprintFee").click(function () {
        var data = $("#printgrid2").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有任何收费信息", 2000);
            return false;
        }
        $.messager.confirm("", "确定要打印吗？", function (yes) {
            if (yes) {//Easy.AddProvePrintMoreContent
                var feeIdString = "";
                for (var i = 0; i < data.rows.length; i++) {
                    feeIdString += data.rows[i].FeeID + ',';
                }
                feeIdString = feeIdString.substring(0, feeIdString.length - 1);
                var result = Easy.bindSelectInfo("../Fee/ValidateFee", feeIdString);
                if (result.IsError === true) {
                    Easy.centerShow("系统消息", result.Message, 2000);
                    return false;
                }
                else {
                    var feeData = JSON.parse(result.Data);
                    var html = Easy.AddProvePrintMoreContent(feeData[0], result.Message);
                    var LODOP; //声明为全局变量
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("证书费收据");
                    LODOP.SET_PRINTER_INDEX(-1);
                    LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                    LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                    LODOP.PREVIEW();
                    $("#printFee").dialog('close');
                }
            }
        });
    });
};