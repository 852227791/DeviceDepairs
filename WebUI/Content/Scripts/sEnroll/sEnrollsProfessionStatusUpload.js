
bindFormEvent = function () {
    Easy.UpLoadFilePreview("#file_upload1", 'uploadChafileQueue', "#gridsEnroll", "#Path", "../sEnrollsProfessionStatusUpload/CheckExcelModel", "#chaLayout", "在校状态变更信息");
    $("#upDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#upDept').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
        }
    });
}

bindStudentInfo = function () {
    $("#gridsEnroll").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        columns: [[
            { field: "x", checkbox: true },
            { field: "系统备注", title: "系统备注", hidden: true },
            { field: "姓名", title: "姓名", width: 95, sortable: false },
            { field: "身份证号", title: "身份证号", width: 95, sortable: false },
            { field: "学历层次", title: "学历层次", width: 60, sortable: false },
            { field: "专业", title: "专业", width: 155, sortable: false },
            { field: "在校状态", title: "在校状态", width: 80, sortable: false },
            { field: "变更理由", title: "变更理由", width: 100, sortable: false }
        ]],
        sortName: "Profession", sortOrder: "desc",
        toolbar: [{
            iconCls: 'icon-edit',
            text: '删除',
            handler: function () {
                var rows = $("#gridsEnroll").datagrid("getSelections");//选中的所有ID
                if (Easy.checkRows(rows, "在校状态变更信息")) {
                    $.messager.confirm("", "确定要删除吗？", function (yes) {
                        if (yes) {
                            for (var i = 0; i < rows.length; i++) {
                                var index = $("#gridsEnroll").datagrid('getRowIndex', rows[i]);
                                $("#gridsEnroll").datagrid('deleteRow', index);
                            }
                        }
                    });
                }
            }
        }, '-', {
            iconCls: 'icon-large-smartart',
            text: '清空',
            handler: function () {
                $.messager.confirm("", "确定要清空吗？", function (yes) {
                    if (yes) {
                        $("#gridsEnroll").datagrid({ data: [] });
                        $("#Path").textbox("setValue", "");
                        $("#gridsEnroll").datagrid("hideColumn", "系统备注");
                    }
                });
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '导出',
            handler: function () {
                Easy.DeriveFile("#gridsEnroll", "在校状态变更信息.xls");
            }
        }],
        rowStyler: function (index, row) {
            if (row.系统备注 != undefined) {
                return "color:#ff0000;";
            }
        }
    });
}
bindStudentInfo();
bindFormEvent();