bindFormEvent = function () {
    loadAddDeptArea(defaultDeptID);
    DeptID = defaultDeptID;
    $("#AddDeptID").combotree({
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
                $('#AddDeptID').combotree('clear');
                $('#AddDeptAreaID').combobox('clear');
                $('#AddDeptAreaID').combobox('loadData', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                DeptID = node.id;
                loadAddDeptArea(DeptID);
            }
            $("#AddChooseItemDetail").textbox("setValue", "");
            $("#AddShouldMoney").numberspinner("setValue", "");
            $("#AddPaidMoney").numberspinner("setValue", "");
            $("#AddDiscountMoney").numberspinner("setValue", 0);
            $("#AddOffsetMoney").numberspinner("setValue", 0);
        }
    });

    $("#AddDeptAreaID").combobox({ editable: false });

    $("#btnAddChooseStudent").click(function () {
        var t = $('#AddDeptID').combotree('tree');//获取树对象
        var nodes = t.tree('getSelected');
        if (Easy.checkNode(nodes, "收费单位")) {
            textboxId = "#AddName";
            dialogId = "#chooseStudent";
            deptId, IdTextbox = "#AddStudentID";
            deptId = nodes.id;
            Easy.OpenDialogEvent("#chooseStudent", "选择人员", 800, 500, "../Student/ChooseStudentAdd", "#chooseStudent-buttons");
        }
    });

    $("#btnSaveStudent").click(function () {
        var rows = $("#choosestudentGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "人员")) {
            $("#AddStudentID").textbox("setValue", rows[0].StudentID);
            $("#AddName").textbox("setValue", rows[0].Name + "_" + rows[0].IDCard);
            $("#chooseStudent").dialog('close');
        }
    });

    $("#AddPersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#AddFeeTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });

    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "feetime")) {
        $("#AddFeeTime").datebox({
            readonly: true
        });
    }

    $("#btnAddSetiFee").click(function () {
        var t = $('#AddDeptID').combotree('tree');//获取树对象
        var nodes = t.tree('getSelected');
        if (Easy.checkNode(nodes, "收费单位")) {
            Easy.OpenDialogEvent("#addchooseiFee", "选择收费项目", 900, 600, "../iFee/ChooseiFeeListMore", "#addchooseiFee-buttons");
        }
    });

    $("#btnAddSaveiFee").click(function () {
        $("#itemDetailGrid").edatagrid('saveRow');
        var data = $('#itemDetailGrid').datagrid("getData");
        if (data.rows.length > 0) {
            var tips = "";
            var shouldmoney = 0;
            var paidmoney = 0;
            var discountmoney = 0;
            var offsetmoney = 0;
            for (var i = 0; i < data.rows.length; i++) {
                shouldmoney += parseFloat(data.rows[i].ShouldMoney);
                paidmoney += parseFloat(data.rows[i].PaidMoney);
                discountmoney += parseFloat(data.rows[i].DiscountMoney);
                offsetmoney += parseFloat(data.rows[i].OffSetMoney);
                if (data.rows[i].FeeMode == "") {
                    tips = "请选择 " + data.rows[i].ItemDetailName + " 的缴费方式";
                    break;
                }
                if (parseFloat(data.rows[i].ShouldMoney) == 0) {
                    tips = "" + data.rows[i].ItemDetailName + " 的应收金额不能为0";
                    break;
                }
                if (parseFloat(data.rows[i].ShouldMoney) != parseFloat(data.rows[i].PaidMoney) + parseFloat(data.rows[i].DiscountMoney) + parseFloat(data.rows[i].OffSetMoney)) {
                    tips = "" + data.rows[i].ItemDetailName + " 的应收金额必须等于实收金额+优惠金额+充抵金额";
                    break;
                }
            }
            if (tips != "") {
                Easy.centerShow("系统消息", tips, 3000);
                return false;
            }
            else {
                $("#AddChooseItemDetail").textbox("setValue", JSON.stringify(data));
                $("#AddShouldMoney").numberspinner("setValue", shouldmoney);
                $("#AddPaidMoney").numberspinner("setValue", paidmoney);
                $("#AddDiscountMoney").numberspinner("setValue", discountmoney);
                $("#AddOffsetMoney").numberspinner("setValue", offsetmoney);
            }
        }
        else {
            Easy.centerShow("系统消息", "请选择项目信息", 1000);
            return false;
        }
        $("#addchooseiFee").dialog('close');
    });
}

loadAddDeptArea = function (deptId) {
    $("#AddDeptAreaID").combobox({
        url: "../DeptArea/GetDeptAreaCombobox?DeptID=" + deptId,
        valueField: "id",
        textField: "name",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

bindFormEvent();
