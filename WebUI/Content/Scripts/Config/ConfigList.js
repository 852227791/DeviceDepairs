
firstFunction = function () {
    initTable({});
    Easy.bindSaveButtonClickEvent("#editConfig", "#fSave", "../Config/GetConfigEdit", "#btnSave", "1", "#grid", menuId, "1", "#ConfigID");
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ConfigID", hidden: true },
            { field: "VoucherNum", title: "凭证号", sortable: true, halign: "left", align: "right" },
            { field: "NoteNum", title: "票据号", sortable: true, halign: "left", align: "right" },
            { field: "PrintNum", title: "打印次数", sortable: true, halign: "left", align: "right" }
        ]],
        url: "../Config/GetConfigList", sortName: "ConfigID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

//绑定搜索表单数据



//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "基础信息")) {
            Easy.OpenDialogEvent("#editConfig", "编辑基础信息", 400, 220, "../Config/ConfigEdit", "#editConfig-buttons");
        }
    }
}

