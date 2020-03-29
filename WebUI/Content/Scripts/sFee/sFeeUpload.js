
bindFormEvent = function () {
    Easy.UpLoadFilePreview("#file_upload", 'uploadfileQueue', "#gridStudent", "#filePath", "../sFee/GetExcelData", "#stuLayout", "交费人信息");
    $("#Dept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#Dept').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
        }
    });
}

bindStudentInfo = function () {
    $("#gridStudent").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        columns: [[
            { field: "x", checkbox: true },
            { field: "系统备注", title: "系统备注", hidden: true },
            { field: "姓名", title: "姓名", width: 95, sortable: false },
            { field: "身份证号", title: "身份证号", width: 95, sortable: false },
            { field: "学历层次", title: "学历层次", width: 60, sortable: false },
            { field: "专业", title: "专业", width: 135, sortable: false },
            { field: "缴费学年", title: "缴费学年", width: 40, sortable: false },
            { field: "费用类别", title: "费用类别", width: 100, sortable: false },
            { field: "缴费类别", title: "缴费类别", width: 80, sortable: false },
            { field: "供款金额", title: "供款金额", width: 80, sortable: false },
            { field: "缴费方式", title: "缴费方式", width: 80, sortable: false },
            { field: "打印说明", title: "打印说明", width: 80, sortable: false },
            { field: "备注", title: "备注", width: 80, sortable: false }
        ]],
        sortName: "Profession", sortOrder: "desc",
        toolbar: [{
            iconCls: 'icon-edit',
            text: '删除',
            handler: function () {
                var rows = $("#gridStudent").datagrid("getSelections");//选中的所有ID
                if (Easy.checkRows(rows, "学费缴费信息")) {
                    $.messager.confirm("", "确定要删除吗？", function (yes) {
                        if (yes) {
                            for (var i = 0; i < rows.length; i++) {
                                var index = $("#gridStudent").datagrid('getRowIndex', rows[i]);
                                $("#gridStudent").datagrid('deleteRow', index);
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
                        $("#gridStudent").datagrid({ data: [] });
                        $("#filePath").textbox("setValue", "");
                        $("#gridStudent").datagrid("hideColumn", "系统备注");
                    }
                });
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '导出',
            handler: function () {
                Easy.DeriveFile("#gridStudent", "学费缴费信息.xls");
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