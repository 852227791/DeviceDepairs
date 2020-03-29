var iFeeID = "0";//定义ID
var DeptID = "0 ";
var textboxId, dialogId, deptId, IdTextbox;
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editiFee", "#fSave", "../iFee/GetiFeeEdit", "#btnSave", "1", "#grid", menuId, "1", "#iFeeID");
    bindSaveUplodMethod();
    //  Easy.bindSaveUploadFile("#upiFee", "#fUpload", "../iFee/UpLoadiFee", "#btnSaveupiFee", "1", "#grid", menuId, "1", "upload");

    Easy.bindSaveButtonClickEvent("#addiFee", "#fAddSave", "../iFee/GetiFeeAdd", "#btnAddSave", "1", "#grid", menuId, "1", "#AddiFeeID");

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
            { field: "iFeeID", checkbox: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "Dept", title: "单位", width: 200, sortable: true },
            { field: "DeptAreaName", title: "收费校区", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "EnrollNum", title: "学号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            {
                field: "FeeParentContent", title: "收费项目", width: 150, sortable: true,
                formatter: function (value, row, index) {
                    if (value != "" && value != undefined) {
                        return value.substring(0, value.length - 1);
                    }

                }
            },
            { field: "FeeContent", title: "收费类别", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "FeeMode", title: "缴费方式", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", sortable: true, halign: 'left', align: 'right' },
            { field: "PaidMoney", title: "实缴金额", sortable: true, halign: 'left', align: 'right' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, halign: 'left', align: 'right' },
            {
                field: "OffsetMoney", title: "充抵金额", sortable: true, halign: 'left', align: 'right',
                formatter: function (value, row, index) {
                    if (row.OffsetStr != "" && row.OffsetStr != null) {
                        return "<span title=\"" + row.OffsetStr.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            {
                field: "BeOffsetMoney", title: "被充抵金额", sortable: true, halign: 'left', align: 'right',
                formatter: function (value, row, index) {
                    if (row.BeOffsetStr != "" && row.BeOffsetStr != null) {
                        return "<span title=\"" + row.BeOffsetStr.replace(/,/g, "<br />") + "\" class=\"tip\" style=\"text-decoration: underline;cursor: pointer;\">" + value + "</span>";
                    }
                    else {
                        return value;
                    }
                }
            },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "CreateName", title: "收费人", width: 60, sortable: true },
            { field: "PersonSort", title: "交款人员", width: 60, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "Affirm", title: "结账人", width: 60, sortable: true },
            { field: "AffirmTime", title: "结账时间", width: 80, sortable: true },
            { field: "Explain", title: "打印说明", width: 200, sortable: true },
            { field: "Remark", title: "系统备注", width: 200, sortable: true }
        ]],
        url: "../iFee/GetiFeeList", sortName: "iFeeID", sortOrder: "desc",
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
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
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
        panelWidth: 300,
        onSelect: function (node) {
            loadSelectDeptArea(node.id);
        }
    });
    $("#selFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=7",
        valueField: "Value",
        textField: "RefeName",
        panelWidth: 100,
        multiple: true
    });
    $("#txtFeeTimeS").datebox({});
    $("#txtFeeTimeE").datebox({});
    $("#selPersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });

    $("#txtFeeName").combotree({
        url: "../ItemDetail/ItemDetailAllCombotree",
        queryParams: { DeptID: 77, Type: "2" },
        animate: true,
        panelWidth: 300,
        lines: true,
        multiple: false,
        editable: true,
        keyHandler: {
            query: queryHandlerFeeName
        },
        onHidePanel: function (node) {
            var nodeTree = $('#txtFeeName').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#txtFeeName").combotree("setValue", nodeTree.id);
            }
        }
    });
}

queryHandlerFeeName = function (searchText, event) {
    $('#txtFeeName').combotree('tree').tree("search", searchText);
}

loadSelectDeptArea = function (deptId) {
    $("#selDeptAreaID").combobox({
        url: "../DeptArea/GetDeptAreaCombobox?DeptID=" + deptId,
        valueField: "id",
        textField: "name",
        panelHeight: 120,
        multiple: true,
        editable: false
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        iFeeID = "0";
        Easy.OpenDialogEvent("#editiFee", "添加杂费信息", 680, 470, "../iFee/iFeeEdit", "#editiFee-buttons");
    }
}

//添加（多项）
addMore = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "addMore")) {
        iFeeID = "0";
        Easy.OpenDialogEvent("#addiFee", "批量添加杂费信息", 680, 300, "../iFee/iFeeAddMore", "#addiFee-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费信息")) {
            iFeeID = rows[0].iFeeID;
            Easy.OpenDialogEvent("#editiFee", "编辑杂费信息", 680, 470, "../iFee/iFeeEdit", "#editiFee-buttons");
        }
    }
}

//导出收费模板
deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/杂费收费导入模板.xls";
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
            rowsIdStr += rows[i].iFeeID + ",";
        }
        if (rowsIdStr != "") {
            rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
        }
        //批量修改状态
        Easy.bindUpdateValues(confirmstr, status, "../iFee/GetUpdatesStatus", rowsIdStr, "1", "#grid");
    }
}

bindResetPrintNum = function (confirmstr, printNum) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRows(rows, "收费信息")) {
        var rowsIdStr = "";
        for (var i = 0; i < rows.length; i++) {
            rowsIdStr += rows[i].iFeeID + ",";
        }
        if (rowsIdStr != "") {
            rowsIdStr = rowsIdStr.substring(0, rowsIdStr.length - 1);
        }
        //重置打印次数
        Easy.bindUpdateValues(confirmstr, printNum, "../iFee/GetResetPrintNum", rowsIdStr, "1", "#grid");
    }
}

//打印
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
                        var result = Easy.bindSelectInfo("../iFee/GetPrintInfo", rows[i].iFeeID);
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
                                url: "../iFee/UpdateNoteNum",
                                async: false,
                                data: { iFeeID: model.iFeeID },
                                dataType: "json",
                                success: function (result) {
                                    if (result.IsError === false) {
                                        $("#grid").datagrid("load");
                                        noteNum = result.Message;

                                        LODOP.PRINT_INIT("杂费收据");
                                        LODOP.SET_PRINTER_INDEX(-1);
                                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 1000, "");
                                        var html = Easy.AddIncPrintContent(model.ParentDeptName, model.VoucherNum, noteNum, model.DeptName, model.StudentName, model.IDCard, model.FeeMode, model.FeeTime, model.ItemName, model.ShouldMoney, model.Remark1 + model.Remark2 + model.Remark3 + model.Remark4 + model.Remark5 + model.Remark, model.Feeer);
                                        LODOP.ADD_PRINT_HTM(20, 0, 450, 370, html);
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

//重新打印
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
                        var result = Easy.bindSelectInfo("../iFee/GetPrintInfoNoPrintNum", rows[i].iFeeID);
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
                                url: "../iFee/UpdateNoteNum",
                                async: false,
                                data: { iFeeID: model.iFeeID },
                                dataType: "json",
                                success: function (result) {
                                    if (result.IsError === false) {
                                        $("#grid").datagrid("load");
                                        noteNum = result.Message;

                                        LODOP.PRINT_INIT("杂费收据");
                                        LODOP.SET_PRINTER_INDEX(-1);
                                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 1000, "");
                                        var html = Easy.AddIncPrintContent(model.ParentDeptName, model.VoucherNum, noteNum, model.DeptName, model.StudentName, model.IDCard, model.FeeMode, model.FeeTime, model.ItemName, model.ShouldMoney, model.Remark1 + model.Remark2 + model.Remark3 + model.Remark4 + model.Remark5 + model.Remark, model.Feeer);
                                        LODOP.ADD_PRINT_HTM(20, 0, 450, 370, html);
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

//重置打印次数
reset = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "reset")) {
        bindResetPrintNum("重置", "0");
    }
}

rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtName: $("#txtName").textbox("getValue"),
        EnrollNum: $("#txtEnrollNum").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        txtVoucherNum: $("#txtVoucherNum").textbox("getValue"),
        txtNoteNum: $("#txtNoteNum").textbox("getValue"),
        txtCreateName: $("#txtCreateName").textbox("getValue"),
        treeDept: $("#treeDept").combotree("getValue"),
        selFeeMode: "" + $("#selFeeMode").combobox("getValues"),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        txtFeeTimeS: $("#txtFeeTimeS").datebox("getValue"),
        txtFeeTimeE: $("#txtFeeTimeE").datebox("getValue"),
        txtFeeName: $("#txtFeeName").textbox("getText"),
        selPersonSort: "" + $("#selPersonSort").combobox("getValues"),
        selDeptAreaID: "" + $("#selDeptAreaID").combobox("getValues")
    }
    return queryData;
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../iFee/DownloadiFee",
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

//导入
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#upiFee", "导入杂费信息", 680, 550, "../iFee/iFeeUp", "#upiFee-buttons");
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "杂费信息")) {
            Easy.OpenDialogEvent("#iFeeDetail", "查看杂费信息", 800, 600, "../iFee/iFeeDetail", "#iFeeDetail-buttons");
        }
    }
}
bindSaveUplodMethod = function () {
    $("#btnSaveupiFee").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有学生信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload", "../iFee/UploadiFee", "#btnSaveupiFee", "#grid", "1", "upload", "#gridStudent", data.rows, "#ifeeLayout");
    });
}

printMore = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "printMore")) {
        Easy.OpenDialogEvent("#printFee", "合并打印杂费收费", 800, 600, "../iFee/iFeePrint", "#printFee-buttons");
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
                    feeIdString += data.rows[i].iFeeID + ',';
                }
                feeIdString = feeIdString.substring(0, feeIdString.length - 1);
                var result = Easy.bindSelectInfo("../iFee/ValidateiFee", feeIdString);
                if (result.IsError === true) {
                    Easy.centerShow("系统消息", result.Message, 2000);
                    return false;
                }
                else {
                    var feeData = JSON.parse(result.Data);
                    var html = Easy.iFeePrintMoreContent(feeData[0], result.Message);
                    var LODOP; //声明为全局变量
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("杂费收据");
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

printByStudent = function () {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    var flag = false;
    var feeId = "";

    if (Easy.checkRows(rows, "杂费信息")) {
        for (var i = 0; i < rows.length; i++) {
            var row = rows[0];
            if (rows[i].IDCard != row.IDCard || rows[i].FeeTime != row.FeeTime || rows[i].Dept != row.Dept) {
                flag = true;
                break;
            }
            feeId += rows[i].iFeeID + ",";
        }
        if (flag) {
            Easy.centerShow("系统消息", "收费人不同或者缴费时间不同，不能使用此打印", 2000);
            return false;
        }
        else {
            feeId = feeId.substring(0, feeId.length - 1);
            $.messager.confirm("", "确定要打印吗？", function (yes) {
                if (yes) {
                    var result = Easy.bindSelectInfo("../iFee/ValidatePrintByStudent", feeId);
                    if (result.IsError === true) {
                        Easy.centerShow("系统消息", result.Message, 2000);
                        return false;
                    }
                    else {
                        var feeData = JSON.parse(result.Data);
                        var html = Easy.iFeePrintBySameMoreContent(feeData[0], result.Message);
                        var LODOP; //声明为全局变量
                        LODOP = getLodop();
                        LODOP.PRINT_INIT("杂费收据");
                        LODOP.SET_PRINTER_INDEX(-1);
                        LODOP.SET_PRINT_PAGESIZE(1, 1200, 2000, "");
                        LODOP.ADD_PRINT_HTM(0, 0, 450, 750, html);
                        LODOP.PREVIEW();
                    }
                }
            });
        }
    }
};
rePrintByStudent = function () {


};