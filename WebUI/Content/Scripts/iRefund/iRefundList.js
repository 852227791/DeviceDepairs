var iRefundID = "0";

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
    Easy.bindSaveButtonClickEvent("#editiRefund", "#fSave", "../iRefund/GetiRefundEdit", "#btnSave", "1", "#grid", menuId, "1", "#iRefundID");
    Easy.bindSaveUploadFile("#upiRefund", "#fUpload", "../iRefund/UpLoadiRefund", "#btnSaveupiRefund", "1", "#grid", menuId, "1", "upload");
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
            { field: "iRefundID", hidden: true },
             { field: "DeptName", title: "收费单位", width: 120, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 80, sortable: true },
            { field: "Name", title: "缴费人", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "FeeContent", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "核销类别", width: 80, sortable: true },
            { field: "RefundMoney", title: "核销金额", sortable: true, halign: 'left', align: 'right' },
            { field: "RefundTime", title: "核销时间", width: 80, sortable: true },
            { field: "PayObject", title: "支付对象", width: 80, sortable: true },
            { field: "CreateName", title: "核销员", sortable: true, width: 80 },
            { field: "Remark", title: "核销备注", sortable: true, width: 200 }
        ]],
        url: "../iRefund/GetiRefundList", sortName: "iRefundID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "2") {
                return "color:#ff0000;";
            }
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
    $("#selSort").combobox({
        url: "../Refe/SelList?RefeTypeID=10",
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
}

//启用
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        iRefundID = "0";
        Easy.OpenDialogEvent("#editiRefund", "添加杂费核销", 680, 300, "../iRefund/iRefundEdit", "#editiRefund-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "核销信息")) {
            iRefundID = rows[0].iRefundID;
            Easy.OpenDialogEvent("#editiRefund", "编辑杂费核销", 680, 300, "../iRefund/iRefundEdit", "#editiRefund-buttons");
        }
    }
}

//导出核销模板
deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/杂费核销导入模板.xls";
    }
}

bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的ID
    //验证是否选中行
    if (Easy.checkRow(rows, "核销信息")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../iRefund/UpdateiRefundStatus", rows[0].iRefundID, "1", "#grid");
    }
}

rerurnQueryData = function () {
    
    var queryData = {
        MenuID: menuId,
        txtName: $("#txtName").textbox('getValue'),
        txtVoucherNum: $("#txtVoucherNum").textbox('getValue'),
        txtIDCard: $("#txtIDCard").textbox('getValue'),
        selSort: "" + $("#selSort").combobox("getValues"),
        txtNoteNum: $("#txtNoteNum").textbox('getValue'),
        txtRefundTimeS: $("#txtRefundTimeS").datebox('getValue'),
        txtRefundTimeE: $("#txtRefundTimeE").datebox('getValue'),
        txtDeptID: $("#txtDeptID").combotree('getValue')
    }
    return queryData;
}

//导入
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#upiRefund", "导入杂费核销", 680, 550, "../iRefund/iRefundUp", "#upiRefund-buttons");
    }
}


download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../iRefund/Download",
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