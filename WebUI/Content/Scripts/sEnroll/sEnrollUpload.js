
bindFormEvent = function () {
    Easy.UpLoadFilePreview("#file_upload", 'uploadfileQueue', "#gridStudent", "#filePath", "../sEnroll/GetExcelData", "#stuLayout", "学生信息");
    $("#upDeptId").combotree({
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
                $('#upDeptId').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
                return false;
            }
            $("#upEnrollDeptId").combobox({
                url: "../DeptArea/GetDeptAreaCombobox",
                queryParams: { DeptID: node.id },
                valueField: "id",
                textField: "name",
                panelHeight: 120,
                multiple: false,
                editable: false
            });
        }

    });
    $("#upSort").combobox({
        url: "../Refe/SelList?RefeTypeID=14",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        onChange: function (newValue, oldValue) {
            $("#Name").textbox("validate");
        }
    });

    $("#upYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#upMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

bindStudentInfo = function () {
    $("#gridStudent").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        frozenColumns: [[
            { field: "x", checkbox: true },
            { field: "考生号", title: "考生号", width: 100, sortable: false },
            { field: "姓名", title: "姓名", width: 95, sortable: false },
            { field: "身份证号", title: "身份证号", width: 135, sortable: false }]
        ],
        columns: [[
            { field: "系统备注", title: "系统备注", hidden: true },
            { field: "招生部门", title: "招生部门", width: 60, sortable: false },
            { field: "性别", title: "性别", width: 60, sortable: false },
            { field: "手机号", title: "手机号", width: 135, sortable: false },
            { field: "QQ", title: "QQ", width: 40, sortable: false },
            { field: "微信", title: "微信", width: 100, sortable: false },
            { field: "地址", title: "地址", width: 80, sortable: false },
            { field: "民族", title: "民族", width: 80, sortable: false },
            { field: "生源地_省", title: "生源地_省", width: 80, sortable: false },
            { field: "生源地_市", title: "生源地_市", width: 80, sortable: false },
            { field: "生源地_县", title: "生源地_县", width: 80, sortable: false },
            { field: "邮政编码", title: "邮政编码", width: 80, sortable: false },
            { field: "父母姓名", title: "父母姓名", width: 80, sortable: false },
            { field: "父母电话", title: "父母电话", width: 80, sortable: false },
            { field: "学习层次", title: "学习层次", width: 80, sortable: false },
            { field: "录取专业", title: "录取专业", width: 80, sortable: false }
        ]],
        sortName: "Profession", sortOrder: "desc",
        toolbar: [{
            iconCls: 'icon-edit',
            text: '删除',
            handler: function () {
                var rows = $("#gridStudent").datagrid("getSelections");//选中的所有ID
                if (Easy.checkRows(rows, "交费人信息")) {
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
                Easy.DeriveFile("#gridStudent", "报名信息.xls");
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