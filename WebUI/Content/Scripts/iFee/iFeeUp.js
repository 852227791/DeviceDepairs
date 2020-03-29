bindFormEvent = function () {
    Easy.UpLoadFilePreview("#file_upload", 'uploadfileQueue', "#gridStudent", "#filePath", "../iFee/GetExcelData", "#ifeeLayout", "缴费人信息");
    loadUpDeptArea(defaultDeptID);
    setTimeout(function () {
        loadItemTree(defaultDeptID);
    }, 1);
    $("#DeptID").combotree({
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
                DeptID = "";
                $('#DeptID').combotree('clear');
                $('#DeptAreaID').combobox('clear');
                $('#DeptAreaID').combobox('loadData', '');
                $('#ItemDetailID').combotree('clear');
                $('#ItemDetailID').combotree('loadData', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                DeptID = node.id;
                loadUpDeptArea(DeptID);
                loadItemTree(DeptID);
            }
        }
    });

    $("#DeptAreaID").combobox({ editable: false });

    $("#PersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
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

    $("#FeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#ShouldMoney").numberspinner({
        readonly: true
    });
}

loadUpDeptArea = function (deptId) {
    $("#DeptAreaID").combobox({
        url: "../DeptArea/GetDeptAreaCombobox?DeptID=" + deptId,
        valueField: "id",
        textField: "name",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

loadItemTree = function (deptId) {
    $("#ItemDetailID").combotree({
        url: "../ItemDetail/ItemDetailAllCombotree",
        queryParams: { DeptID: deptId, Type: "2" },
        animate: true,
        panelWidth: 300,
        lines: true,
        multiple: false,
        editable: true,
        onlyLeafCheck: true,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#ItemDetailID').combotree('clear');
                queryHandler("", null);
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var obj = getItemDetail(node.id);
                if (obj != null && obj != undefined) {
                    if (obj.Sort == 1) {
                        //固定金额，应收不可编辑。
                        $("#ShouldMoney").numberspinner({
                            readonly: true
                        });
                        $("#ShouldMoney").numberspinner("setValue", obj.Money);
                    }
                    else {
                        //设置下限金额。应收可编辑，并且必须大于下限金额
                        $("#ShouldMoney").numberspinner({
                            readonly: false
                        });
                        $("#ShouldMoney").numberspinner("setValue", 0);
                    }
                }
                else {
                    $('#ItemDetailID').combotree('clear');
                    queryHandler("", null);
                    $("#ShouldMoney").numberspinner("setValue", 0);
                    Easy.centerShow("系统消息", "未设置收费明细", 3000);
                }
                $("#PaidMoney").numberspinner("setValue", 0);
                $("#DiscountMoney").numberspinner("setValue", 0);
            }
        },
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#ItemDetailID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#ItemDetailID").combotree("setValue", nodeTree.id);
            }
        }
    });
}

getItemDetail = function (itemDetailId) {
    itemDetailId = itemDetailId.substring(itemDetailId.indexOf("_") + 1, itemDetailId.length);
    var result = Easy.bindSelectInfo("../ItemDetail/GetMoneyAll", itemDetailId.toString());
    var p = JSON.parse(result.Message)[0];
    return p;
}

queryHandler = function (searchText, event) {
    $('#ItemDetailID').combotree('tree').tree("search", searchText);
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
            { field: "姓名", title: "姓名", width: 60, sortable: false },
            { field: "身份证号", title: "身份证号", width: 135, sortable: false },
            { field: "学号", title: "学号", width: 135, sortable: false },
            { field: "性别", title: "性别", width: 40, sortable: false },
            { field: "手机", title: "手机", width: 100, sortable: false }
        ]],
        sortName: "Profession", sortOrder: "desc",
        toolbar: [{
            iconCls: 'icon-edit',
            text: '删除',
            handler: function () {
                var rows = $("#gridStudent").datagrid("getSelections");//选中的所有ID
                if (Easy.checkRows(rows, "学生信息")) {
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
                Easy.DeriveFile("#gridStudent", "缴费人信息.xls")
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

bindSaveUplodMethod = function () {
    $("#btnSaveupFee").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有学生信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload", "../iFee/GetUpdateProveFee", "#btnSaveupFee", "#grid", "1", "upload", "#gridStudent", data.rows, "#ifeeLayout");
    });
}