bindFormEvent = function () {
    if (FeeID === "0") {
        $("#fSave").form('reset');
    }
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
                $('#editDeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
        }
    });


    $("#editShouldMoney").numberspinner({
        readonly: true
    });
    $("#editOffsetMoney").numberspinner({
        readonly: true
    }); 
    $("#editDiscountMoney").numberspinner({
        readonly: true
    });
    $("#editPersonSort").combobox({
        url: "../Refe/SelList?RefeTypeID=11",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
    $("#editFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
    $("#btnChooseProve").click(function () {
        var t = $('#editDeptID').combotree('tree');	// 获取树对象
        var nodes = t.tree('getSelected');
        if (Easy.checkNode(nodes, "收费单位")) {
            DeptID = nodes.id;
            Easy.OpenDialogEvent("#chooseProve", "选择证书", 600, 420, "../Prove/ProveChoose", "#chooseProve-buttons");
        }
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
    $("#btnChooseOffset").click(function () {
        if (!bindItemDetailJson()) {
            Easy.centerShow("系统消息", "请选择收费项", 3000);
            return false;
        }
        var t = $('#editDeptID').combotree('tree');	// 获取树对象
        var nodes = t.tree('getSelected');
        if (Easy.checkNode(nodes, "收费单位")) {
            DeptID = nodes.id;
            Easy.OpenDialogEvent("#chooseFee", "选择收费信息", 800, 600, "../Fee/ChooseFeeList", "#chooseFee-buttons");
            var data = $("#editOffsetID").val();
            if (data != "") {
                tempData = JSON.parse(data);
            }
        }
    });

    $("#btnChooseDiscount").click(function () {
        if (!bindItemDetailJson()) {
            Easy.centerShow("系统消息", "请选择收费项", 3000);
            return false;
        }
        Easy.OpenDialogEvent("#eidtDiscountMoney", "编辑优惠信息", 600, 300, "../Fee/FeeDiscountEdit", "#eidtDiscountMoney-buttons");
    });

}

bindItemDetailJson = function () {
    var flag = true;
    var id = $("#editItemDetailID").combobox('getValues');
    var text = $("#editItemDetailID").combobox('getText');

    if (id.length <= 0 || id[0] === "") {
        flag = false;
    }
    var textarray = text.split(',');
    var str = "[";
    for (var i = 0; i < id.length; i++) {
        str += "{" + "\"ItemDetailID\":\"" + id[i] + "\",\"OffsetItem\":\"" + textarray[i] + "\",\"DiscountMoney\":\"0.00\"},"
    }
    str = str.substring(0, str.length - 1);
    str += "]";
    $("#editItemDetailJson").val(str);
    return flag;
}
////获取应收金额
getShouldMoney = function (itemDetailId) {
    var result = Easy.bindSelectInfo("../ItemDetail/GetMoney", itemDetailId.toString());
    var p = JSON.parse(result.Message)[0];
    return p.Money;
}

saveProve = function () {
    $("#btnSaveChooseProve").click(function () {
        var rows = $("#gridProveChoose").datagrid("getSelections");
        if (Easy.checkRow(rows, "证书信息")) {
            $("#editName").textbox("setValue", rows[0].StudentName + "_" + rows[0].ItemName + "_" + rows[0].IDCard);
            $("#editProveID").textbox("setValue", rows[0].ProveID);
            loadItemDetailCommbox(rows[0].ItemID);
            bindSetNone();
            $("#chooseProve").dialog('close');
        }
    });
}
loadItemDetailCommbox = function (itemId) {
    $("#editItemDetailID").combobox({
        url: "../ItemDetail/ItemDetailCombobox",
        queryParams: { ItemID: itemId },
        valueField: "id",
        textField: "text",
        panelHeight: 100,
        checkbox: true,
        multiple: true,
        editable: false,
        onSelect: function () {
            var allnodes = $('#editItemDetailID').combobox('getValues');
            var shouldMoney = getShouldMoney(allnodes.toString());
            $("#editShouldMoney").numberspinner("setValue", shouldMoney);
            $("#editPaidMoney").numberspinner("setValue", shouldMoney);
        },
        onUnselect: function () {
            var allnodes = $('#editItemDetailID').combobox('getValues');
            var shouldMoney = getShouldMoney(allnodes.toString());
            $("#editShouldMoney").numberspinner("setValue", shouldMoney);
            $("#editPaidMoney").numberspinner("setValue", shouldMoney);
        }
    });
}
bindSelectInfo = function (feeId) {
    if (feeId != "0") {
        setTimeout(function () {
            var result1 = Easy.bindSelectInfo("../Fee/SelectFee", feeId);

            var fee = JSON.parse(result1.Message)[0];
            $("#editFeeID").textbox("setValue", fee.FeeID);
            $("#editProveID").textbox("setValue", fee.ProveID);
            $("#editDeptID").combotree("setValue", fee.DeptID);
            $("#editName").textbox("setValue", fee.Name);
            $("#editShouldMoney").numberspinner("setValue", fee.ShouldMoney);
            $("#editPaidMoney").numberspinner("setValue", fee.PaidMoney);
            $("#editDiscountMoney").numberspinner("setValue", fee.DiscountMoney);
            $("#editOffsetMoney").numberspinner("setValue", fee.OffsetMoney);
            $("#editFeeMode").combobox("setValue", fee.FeeMode);
            $("#editPersonSort").combobox("setValue", fee.PersonSort);
            $("#editTeacher").textbox("setValue", fee.Teacher);
            $("#editRemark").textbox("setValue", fee.Remark.replace(/<br \/>/g, "\r\n"));
            $("#editExplain").textbox("setValue", fee.Explain.replace(/<br \/>/g, "\r\n"));
            $("#editFeeTime").datebox("setValue", fee.FeeTime);
            loadItemDetailCommbox(fee.ItemID);
            var result2 = Easy.bindSelectInfo("../FeeDetail/GetItemDetailIdByFeeID", feeId);

            $("#editItemDetailID").combobox("setValues", result2.Data);
            // $("#btnChooseOffset").hide();
            var result3 = Easy.bindSelectInfo("../Fee/GetSelectItemDetailOffset", feeId);
            $("#editOffsetID").textbox("setValue", result3.Data);
            var result4 = Easy.bindSelectInfo("../Fee/GetSelectDiscount", feeId);
            $("#editFeeDiscountJson").textbox("setValue", result4.Data);
        }, 1);
    }
    else {
        $("#btnChooseOffset").show();
    }
}

bindSaveChooseFee = function () {
    $("#btnSaveFee").click(function () {
        var data = $('#choosefeegrid2').datagrid("getData");
        if (data.rows.length > 0) {
            var offsetMoney = 0;
            for (var i = 0; i < data.rows.length; i++) {
                offsetMoney += parseFloat(data.rows[i].Offset);
            }
            var data2 = JSON.stringify(data);
            $("#editOffsetID").textbox("setValue", data2);
            $("#editOffsetMoney").numberspinner("setValue", offsetMoney);
        }
        else {
            $("#editOffsetID").textbox("setValue", "");
            $("#editOffsetMoney").numberspinner("setValue", "0.00");
        }
        $("#chooseFee").dialog('close');
    });
}
//保存优惠
bindSaveDiscountMoney = function () {
    $("#btnSaveDiscountMoney").click(function () {
        $("#feediscountList").edatagrid('acceptChanges');//执行保存方法
        var data = $("#feediscountList").edatagrid("getData");
        var money = 0;
        var flag = true;
        for (var i = 0; i < data.rows.length; i++) {
            var textarray = data.rows[i]["OffsetItem"].split(' ');
            if (parseFloat(data.rows[i]["DiscountMoney"]) > parseFloat(textarray[1])) {
                flag = false;
                break;
            }
        }
        if (!flag) {
            Easy.centerShow("系统消息", "优惠金额不能大于收费项金额", 3000);
            return false;
        }
        for (var i = 0; i < data.rows.length; i++) {
            if (data.rows[i]["DiscountMoney"] != null && data.rows[i]["DiscountMoney"] != undefined) {
                money = parseFloat(money) + parseFloat(data.rows[i]["DiscountMoney"]);
            }
        }
        if (parseFloat(money) === 0) {
            Easy.centerShow("系统消息", "请选择收费项", 3000);
            return false;
        }
        $("#editFeeDiscountJson").textbox("setValue", JSON.stringify(data));
        $("#editDiscountMoney").numberspinner("setValue", parseFloat(money).toFixed("2"));
        $("#eidtDiscountMoney").dialog('close');
    });
}

bindSetNull = function () {
    $("#setNone").click(function () {
        bindSetNone();
    });
};
bindSetNone = function () {
    $("#editFeeDiscountJson").textbox("setValue", "[]");
     $("#editOffsetID").textbox("setValue", "[]");
    $("#editShouldMoney").numberspinner("setValue", "0.00");
    $("#editPaidMoney").numberspinner("setValue", "0.00");
    $("#editDiscountMoney").numberspinner("setValue", "0.00");
     $("#editOffsetMoney").numberspinner("setValue", "0.00");
    $("#editItemDetailID").combobox("clear");
};
bindFormEvent();
saveProve();
bindSelectInfo(FeeID);
bindSaveChooseFee();
bindSaveDiscountMoney();
bindSetNull();

