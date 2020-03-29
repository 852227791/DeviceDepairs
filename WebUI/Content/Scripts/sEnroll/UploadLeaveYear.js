
bindFormEvent = function () {
    Easy.UpLoadFilePreview("#yearfile_upload", 'uploadfileQueue4', "#gridInfo", "#yearfilePath", "../EnrollChangeLeaveYear/GetYearExcelData", "#upLeaveYearLayout", "毕业年份信息");
    $("#yearDeptId").combotree({
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
                $('#yearDeptId').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
                return false;
            }
        }
    });

}

bindStudentInfo = function () {
    $("#gridInfo").datagrid({
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
            { field: "毕业年份", title: "毕业年份", width: 80, sortable: false }]
        ],
        sortName: "Profession", sortOrder: "desc",
        toolbar: [{
            iconCls: 'icon-edit',
            text: '删除',
            handler: function () {
                var rows = $("#gridInfo").datagrid("getSelections");//选中的所有ID
                if (Easy.checkRows(rows, "毕业年份信息")) {
                    $.messager.confirm("", "确定要删除吗？", function (yes) {
                        if (yes) {
                            for (var i = 0; i < rows.length; i++) {
                                var index = $("#gridInfo").datagrid('getRowIndex', rows[i]);
                                $("#gridInfo").datagrid('deleteRow', index);
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
                        $("#gridInfo").datagrid({ data: [] });
                        $("#yearfilePath").textbox("setValue", "");
                        $("#gridInfo").datagrid("hideColumn", "系统备注");
                    }
                });
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '导出',
            handler: function () {
                Easy.DeriveFile("#gridInfo", "毕业年份信息.xls");
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