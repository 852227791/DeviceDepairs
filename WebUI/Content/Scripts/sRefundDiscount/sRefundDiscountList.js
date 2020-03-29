var sRefundId = "0";
var feeName;
var refund = 'discount';
firstFunction = function () {
    bindSearchFormEvent();
    bindSearchClickEvent();
    Easy.bindOpenCloseSearchBoxEvent(90);
    initGrid({ MenuID: menuId });
    Easy.bindSaveButtonClickEvent("#editsRefund", "#fSave", "../sRefundDiscount/GetsRefundDiscountEdit", "#btnSave", "1", "#grid", menuId, "1", "#sRefundDiscountID");
    bindSaveUplodMethod();
};
initGrid = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        showFooter: true,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "sRefundDiscountID", hidden: true },
            { field: "DeptName", title: "校区", width: 120, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Content", title: "缴费方案", width: 100, sortable: true },
            { field: "DetailName", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "核销类别", width: 80, sortable: true },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundTime", title: "核销时间", width: 80, sortable: true },
            { field: "PayObject", title: "支付对象", width: 80, sortable: true },
            { field: "Creater", title: "核销员", sortable: true, width: 80 },
            { field: "Remark", title: "核销备注", sortable: true, width: 200 }

        ]],
        url: "../sRefundDiscount/GetsRefundDiscountList", sortName: "sRefundDiscountID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#uploadsRefund", "导入核销信息", 800, 600, "../sRefundDiscount/sRefundDiscountUp", "#uploadsRefund-buttons");
    }
};
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        Easy.OpenDialogEvent("#editsRefund", "编辑核销信息", 680, 300, "../sRefundDiscount/sRefundDiscountEdit", "#editsRefund-buttons");
    }
};
edit = function () {
    Easy.centerShow("系统消息", "核销信息不可修改", 3000);
};
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
};
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
};
deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/学费核销导入模板.xls";
    }
};

rerurnQueryData = function () {
    var queryData = {
        MenuID: menuId,
        studentName: $("#txtName").textbox('getValue'),
        voucherNum: $("#txtVoucherNum").textbox('getValue'),
        idCard: $("#txtIDCard").textbox('getValue'),
        selsort: "" + $("#selSort").combobox("getValues"),
        noteNum: $("#txtNoteNum").textbox('getValue'),
        refundTimeS: $("#txtRefundTimeS").datebox('getValue'),
        refundTimeE: $("#txtRefundTimeE").datebox('getValue'),
        txtDeptID: $("#txtDeptID").combotree('getValue')
    }
    return queryData;
}
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initGrid(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initGrid({ MenuID: menuId });
    });
}
bindSearchFormEvent = function () {
    $("#selSort").combobox({
        url: "../Refe/SelList?RefeTypeID=10",
        valueField: "Value",
        textField: "RefeName",
        multiple: true
    });
    $("#txtDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
}
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "核销信息")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../sRefundDiscount/UpdateStatus", rows[0].sRefundDiscountID, "1", "#grid");
    }
}

download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../sRefundDiscount/Download",
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
bindSaveUplodMethod = function () {
    $("#btnSaveUploadsRefund").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "核销信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload", "../sRefundDiscount/UploadsRefundDiscount", "#btnSaveUploadsRefund", "#grid", "1", "upload", "#gridStudent", data.rows, "#stuLayout");
    });
}