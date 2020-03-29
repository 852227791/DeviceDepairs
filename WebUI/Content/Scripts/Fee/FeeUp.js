bindFormEvent = function () {
    Easy.UpLoadFilePreview("#file_upload", 'uploadfileQueue', "#gridStudent", "#filePath", "../Fee/GetExcelData", "#feeLayout", "交费人信息");
    $("#Dept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        panelWidth:280,
        value: defaultDeptID,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#Dept').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            loadItemTree(node.id);
        }
    });

    $("#PersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
    $("#FeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    loadItemTree(defaultDeptID);
    loadItemDetailTree("0");
    $("#Money").numberspinner({
        readonly: true
    });

    $("#EnrollTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });
    $("#FeeTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });
    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "feetime")) {
        $("#FeeTime").datebox({
            readonly: true
        });
    }
}

loadItemTree = function (deptId) {
    $("#ItemID").combotree({
        url: "../Item/ItemCombotree",
        queryParams: { DeptID: deptId ,Type:"1"},
        animate: true,
        panelWidth: 300,
        lines: true,
        multiple: true,
        width:186,
        onlyLeafCheck:true,
        onCheck: function (node, checked) {
            loadItemDetailTree();
            //var tree = $(this).tree;
            //var isLeaf = tree('isLeaf', node.target);
            //if (!isLeaf) {
            //    $('#ItemID').combotree('clear');
            //    Easy.centerShow("系统消息", "请选择末级节点", 3000);
            //}
         
        }
    });
}

getShouldMoney = function (itemDetailId) {
    var result = Easy.bindSelectInfo("../ItemDetail/GetMoney", itemDetailId);
    var p = JSON.parse(result.Message)[0];
    return p.Money;
}
loadItemDetailTree = function () {
    var itemId = $('#ItemID').combotree('getValues');
    $("#ItemDetailID").combotree({
        url: "../ItemDetail/ItemDetailCombotree",
        queryParams: { ItemID: itemId.toString() },
        valueField: "id",
        textField: "text",
        panelHeight: 200,
        panelWidit: 150,
        checkbox: true,
        multiple: true,
        editable: false,
        onlyLeafCheck: true,
        onCheck: function (node, checked) {
            var allnodes = $('#ItemDetailID').combotree('getValues');
            $("#Money").numberspinner("setValue", getShouldMoney(allnodes.toString()));
        },
        uncheck: function (target) {
            var allnodes = $('#ItemDetailID').combotree('getValues');
            $("#Money").numberspinner("setValue", getShouldMoney(allnodes.toString()));
        }
    });
}

bindFormEvent();
bindStudentInfo = function () {
    $("#gridStudent").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        columns: [[
            { field: "x", checkbox: true },
            { field: "系统备注", title: "系统备注", width: 150, sortable: false, hidden: true },
            { field: "专业", title: "专业", width: 95, sortable: false },
            { field: "班级", title: "班级", width: 95, sortable: false },
            { field: "姓名", title: "姓名", width: 60, sortable: false },
            { field: "身份证号", title: "身份证号", width: 135, sortable: false },
            { field: "性别", title: "性别", width: 40, sortable: false },
            { field: "手机", title: "手机", width: 100, sortable: false },
            { field: "QQ", title: "QQ", width: 80, sortable: false },
            { field: "微信", title: "微信", width: 80, sortable: false },
            { field: "地址", title: "地址", width: 100, sortable: false }
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
                Easy.DeriveFile("#gridStudent", "交费人信息.xls");
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
