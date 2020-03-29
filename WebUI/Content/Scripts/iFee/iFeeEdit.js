
bindFormEvent = function () {
    loadDeptArea(defaultDeptID);
    setTimeout(function () {
        loadItemTree(defaultDeptID);
    }, 1);
    $("#editDeptID").combotree({
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
                $('#editDeptID').combotree('clear');
                $('#editDeptAreaID').combobox('clear');
                $('#editDeptAreaID').combobox('loadData', '');
                $('#editItemDetailID').combotree('clear');
                $('#editItemDetailID').combotree('loadData', '');
                $('#editOffsetMoney').numberspinner('setValue', 0.00);
                $('#editChooseOffSet').textbox('setValue', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                DeptID = node.id;
                $('#editOffsetMoney').numberspinner('setValue', 0.00);
                $('#editChooseOffSet').textbox('setValue', '');
                loadDeptArea(DeptID);
                loadItemTree(DeptID);
            }
        }
    });

    $("#btnChooseStudent").click(function () {
        var t = $('#editDeptID').combotree('tree');//获取树对象
        var nodes = t.tree('getSelected');
        if (Easy.checkNode(nodes, "收费单位")) {
            textboxId = "#Name";
            dialogId = "#chooseStudent";
            deptId, IdTextbox = "#StudentID";
            deptId = nodes.id;
            Easy.OpenDialogEvent("#chooseStudent", "选择人员", 800, 500, "../Student/ChooseStudentAdd", "#chooseStudent-buttons");
        }
    });

    $("#btnSaveStudent").click(function () {
        var rows = $("#choosestudentGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "人员")) {
            $("#StudentID").textbox("setValue", rows[0].StudentID);
            $("#Name").textbox("setValue", rows[0].Name + "_" + rows[0].IDCard);
            $("#chooseStudent").dialog('close');
        }
    });

    $("#editPersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#editFeeTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });

    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "feetime")) {
        $("#editFeeTime").datebox({
            readonly: true
        });
    }

    $("#editFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#editShouldMoney").numberspinner({
        readonly: true
    });

    $("#editOffsetMoney").numberspinner({
        readonly: true
    });

    $("#editDeptAreaID").combobox({ editable: false });

    $("#btnChooseOffset").click(function () {
        var t = $('#editDeptID').combotree('tree');//获取树对象
        var nodes = t.tree('getSelected');
        if (Easy.checkNode(nodes, "收费单位")) {
            Easy.OpenDialogEvent("#chooseiFee", "选择收费信息", 800, 600, "../iFee/ChooseiFeeList", "#chooseiFee-buttons");
        }
    });

    $("#btnSaveiFee").click(function () {
        var data = $('#choosefeegrid2').datagrid("getData");
        if (data.rows.length > 0) {
            var offsetMoney = 0;
            for (var i = 0; i < data.rows.length; i++) {
                offsetMoney += parseFloat(data.rows[i].OffSetMoney);
            }
            $("#editChooseOffSet").textbox("setValue", JSON.stringify(data));
            $("#editOffsetMoney").numberspinner("setValue", offsetMoney);
        }
        else {
            $("#editChooseOffSet").textbox("setValue", "");
            $("#editOffsetMoney").numberspinner("setValue", "0.00");
        }
        $("#chooseiFee").dialog('close');
    });
}

loadDeptArea = function (deptId) {
    $("#editDeptAreaID").combobox({
        url: "../DeptArea/GetDeptAreaCombobox?DeptID=" + deptId,
        valueField: "id",
        textField: "name",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

loadItemTree = function (deptId) {
    $("#editItemDetailID").combotree({
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
                $('#editItemDetailID').combotree('clear');
                queryHandler("", null);
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var obj = getItemDetail(node.id);
                if (obj != null && obj != undefined) {
                    if (node.id.indexOf('i_') > -1) {
                        $('#editItemDetailID').combotree('clear');
                        queryHandler("", null);
                        $("#editShouldMoney").numberspinner("setValue", 0);
                        Easy.centerShow("系统消息", "未设置收费明细", 3000);
                    }
                    else {
                        if (obj.Sort == 1) {
                            //固定金额，应收不可编辑。
                            $("#editShouldMoney").numberspinner({
                                readonly: true
                            });
                            $("#editShouldMoney").numberspinner("setValue", obj.Money);
                        }
                        else {
                            //设置下限金额。应收可编辑，并且必须大于下限金额
                            $("#editShouldMoney").numberspinner({
                                readonly: false
                            });
                            $("#editShouldMoney").numberspinner("setValue", 0);
                        }
                    }
                }
                else {
                    $('#editItemDetailID').combotree('clear');
                    queryHandler("", null);
                    $("#editShouldMoney").numberspinner("setValue", 0);
                    Easy.centerShow("系统消息", "未设置收费明细", 3000);
                }
                $("#editPaidMoney").numberspinner("setValue", 0);
                $("#editDiscountMoney").numberspinner("setValue", 0);
            }
        },
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#editItemDetailID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#editItemDetailID").combotree("setValue", nodeTree.id);
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

bindSelectInfo = function (feeId) {
    if (feeId != "0") {
        setTimeout(function () {
            var result1 = Easy.bindSelectInfo("../iFee/SelectiFee", feeId);
            var fee = JSON.parse(result1.Message)[0];
            $("#iFeeID").textbox("setValue", fee.iFeeID);
            $("#StudentID").textbox("setValue", fee.StudentID);
            $("#editDeptID").combotree("setValue", fee.DeptID);
            loadDeptArea(fee.DeptID);
            loadItemTree(fee.DeptID);
            if (fee.Sort == 1) {
                //固定金额，应收不可编辑。
                $("#editShouldMoney").numberspinner({
                    readonly: true
                });
            }
            else {
                //设置下限金额。应收可编辑。
                $("#editShouldMoney").numberspinner({
                    readonly: false
                });
            }
            $("#editDeptAreaID").combobox("setValue", fee.DeptAreaID);
            $("#Name").textbox("setValue", fee.Name);
            $("#editPersonSort").combobox("setValue", fee.PersonSort);
            $("#editFeeTime").datebox("setValue", fee.FeeTime);
            $("#editItemDetailID").combotree("setValue", "id_" + fee.ItemDetailID);
            $("#editFeeMode").combobox("setValue", fee.FeeMode);
            $("#editShouldMoney").numberspinner("setValue", fee.ShouldMoney);
            $("#editPaidMoney").numberspinner("setValue", fee.PaidMoney);
            $("#editDiscountMoney").numberspinner("setValue", fee.DiscountMoney);
            $("#editOffsetMoney").numberspinner("setValue", fee.OffsetMoney);
            $("#editExplain").textbox("setValue", fee.Explain.replace(/<br \/>/g, "\r\n"));
            $("#editRemark").textbox("setValue", fee.Remark.replace(/<br \/>/g, "\r\n"));
            $("#btnChooseOffset").hide();
        }, 1);
    }
    else {
        $("#btnChooseOffset").show();
    }
}

queryHandler = function (searchText, event) {
    $('#editItemDetailID').combotree('tree').tree("search", searchText);
}


bindFormEvent();
bindSelectInfo(iFeeID);
