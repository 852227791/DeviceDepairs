var RefundID = "0";
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
    Easy.bindSaveButtonClickEvent("#editRefund", "#fSave", "../Refund/GetRefundEdit", "#btnSave", "1", "#grid", menuId, "1", "#RefundID");
    Easy.bindSaveUploadFile("#uploadRefund", "#fUpload", "../Refund/UpLoadRefund", "#btnSaveUploadRefund", "1", "#grid", menuId, "1", "upload");
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "RefundID", hidden: true },
             { field: "DeptName", title: "校区", width: 100, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
             { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "ItemName", title: "证书名称", width: 100, sortable: true },
            { field: "ItemDetailName", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "核销类别", width: 80, sortable: true },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundTime", title: "核销时间", width: 80, sortable: true },
            { field: "PayObject", title: "支付对象", width: 80, sortable: true },
            { field: "Creater", title: "核销员", sortable: true, width: 80 },
            { field: "Status", title: "状态", sortable: true, width: 80 },
            { field: "Remark", title: "核销备注", sortable: true, width: 200 }

        ]],
        url: "../Refund/GetRefundList", sortName: "RefundID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}
rerurnQueryData = function () {
    var queryData = {
        MenuID: menuId,
        studentName: $("#txtName").textbox('getValue'),
        voucherNum: $("#txtVoucherNum").textbox('getValue'),
        idCard: $("#txtIDCard").textbox('getValue'),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        selsort: "" + $("#selSort").combobox("getValues"),
        noteNum: $("#txtNoteNum").textbox('getValue'),
        refundTimeS: $("#txtRefundTimeS").datebox('getValue'),
        refundTimeE: $("#txtRefundTimeE").datebox('getValue'),
        txtDeptID: $("#txtDeptID").combotree("getValue"),
    }
    return queryData;
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
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=1",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });
    $("#txtDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
    $("#selSort").combobox({
        url: "../Refe/SelList?RefeTypeID=10",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        RefundID = "0";
        Easy.OpenDialogEvent("#editRefund", "编辑核销信息", 680, 300, "../Refund/RefundEdit", "#editRefund-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        Easy.centerShow("系统消息", "核销信息不可修改，停用后重新核销", 3000);
        //var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //if (Easy.checkRow(rows, "核销信息")) {
        //    RefundID = rows[0].RefundID;
        //    Easy.OpenDialogEvent("#editRefund", "编辑核销信息", 680, 300, "../Refund/RefundEdit", "#editRefund-buttons");
        //}
    }
}

enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
}
//作废
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
}
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#uploadRefund", "导入核销信息", 400, 300, "../Refund/RefundUpload", "#uploadRefund-buttons");
    }
};
deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/证书核销导入模板.xls";
    }
};
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "核销信息")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Refund/UpdateRefundStatus", rows[0].RefundID, "1", "#grid");
    }
}


download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../Refund/Download",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = result.Data;
                }
            }
        });
    }
}