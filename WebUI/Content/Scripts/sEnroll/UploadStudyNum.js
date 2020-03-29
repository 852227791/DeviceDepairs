
bindFormEvent = function () {
    Easy.UpLoadFilePreview("#study_file", 'uploadfileQueue6', "#gridStudyNum", "#studyNumFilePath", "../sEnroll/GetUploadStudyNumExcel", "#upStudyNumLayout", "学号信息");
    $("#studyNumDeptId").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300,
        value: defaultDeptID,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#studyNumDeptId').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
                return false;
            }
        }
    });

}

bindStudentInfo = function () {
    $("#gridStudyNum").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        columns: [[
            { field: "x", checkbox: true },
            { field: "系统备注", title: "系统备注", hidden: true },
            { field: "姓名", title: "姓名", width: 95, sortable: false },
            { field: "身份证号", title: "身份证号", width: 95, sortable: false },
            { field: "学号", title: "学号", width: 60, sortable: false }]
        ],
        sortName: "Profession", sortOrder: "desc",
        toolbar: [{
            iconCls: 'icon-edit',
            text: '删除',
            handler: function () {
                var rows = $("#gridInfo").datagrid("getSelections");//选中的所有ID
                if (Easy.checkRows(rows, "学号信息")) {
                    $.messager.confirm("", "确定要删除吗？", function (yes) {
                        if (yes) {
                            for (var i = 0; i < rows.length; i++) {
                                var index = $("#gridStudyNum").datagrid('getRowIndex', rows[i]);
                                $("#gridStudyNum").datagrid('deleteRow', index);
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
                        $("#gridStudyNum").datagrid({ data: [] });
                        $("#studyNumFilePath").textbox("setValue", "");
                        $("#gridStudyNum").datagrid("hideColumn", "系统备注");
                    }
                });
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '导出',
            handler: function () {
                Easy.DeriveFile("#gridStudyNum", "学号信息.xls");
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