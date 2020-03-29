
firstFunction = function () {
    //加载表格
    initTable({});
    Easy.bindSaveButtonClickEvent("#editsOrderCreate", "#fSave", "../sOrderCreate/GetsOrderCreateEdit", "#btnSave", "1", "#grid", menuId, "1", "#sOrderCreateID");
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
            { field: "sOrderCreateID", hidden: true },
            { field: "Name", title: "主体名称", width: 300, sortable: true }
        ]],
        url: "../sOrderCreate/GetsOrderCreateList", sortName: "sOrderCreateID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        Easy.OpenDialogEvent("#editsOrderCreate", "编辑为对接迎新主体信息", 300, 280, "../sOrderCreate/sOrderCreateEdit", "#editsOrderCreate-buttons");
    }
}
del = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "del")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "为对接迎新主体信息")) {
            $.messager.confirm('确认', '您确认想要删除吗？', function (r) {
                if (r) {
                    var result = Easy.bindSelectInfo("../sOrderCreate/GetsOrderCreateDel", rows[0].sOrderCreateID);
                    if (result.IsError===false) {
                        $("#grid").datagrid("reload");
                    }
                    Easy.centerShow("系统消息", result.Message, 2000);
                }
            });
        }
    }
}