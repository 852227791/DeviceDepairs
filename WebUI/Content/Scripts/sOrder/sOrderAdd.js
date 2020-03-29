bindAddFormEvent = function () {
    $("#btnChooseStudent").click(function () {
        Easy.OpenDialogEvent("#chooseStudent", "选择学生", 600, 400, "../Student/ChooseStudent", "#chooseStudent-buttons");
    });

    $("#btnSaveStudent").click(function () {
        var rows = $("#choosestudentGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "学生")) {
            $("#StuName").textbox("setValue", rows[0].Name + "_" + rows[0].IDCard);
            $("#chooseStudent").dialog('close');
            $('#sEnrollScheme').combobox('clear');
            $('#sEnrollScheme').combobox('loadData', '');
            $('#SchemeSemester').combobox('clear');
            $('#SchemeSemester').combobox('loadData', '');
            bindsEnrollMajor(rows[0].StudentID);
        }
    });

    $("#btnAddSetsOrder").click(function () {
        Easy.OpenDialogEvent("#choosesOrderDetail", "设置收费项", 630, 400, "../sOrder/SetsOrder", "#choosesOrderDetail-buttons");
    });

    $("#btnSavesOrderDetail").click(function () {
        $("#itemDetailGrid").edatagrid('saveRow');
        var data = $('#itemDetailGrid').datagrid("getData");
        if (data.rows.length > 0) {
            var sName = "";
            var tips = "";
            for (var i = 0; i < data.rows.length; i++) {
                sName += data.rows[i].DetailName + ",";
                if (data.rows[i].Sort == "") {
                    tips = "请选择" + data.rows[i].DetailName + "的项目分类";
                    break;
                }
                if (data.rows[i].IsGive == "") {
                    tips = "请选择" + data.rows[i].DetailName + "的类别";
                    break;
                }
                if (data.rows[i].ShouldMoney.toString() == "") {
                    tips = data.rows[i].DetailName + "的应收金额不能为空";
                    break;
                }
                if (parseFloat(data.rows[i].ShouldMoney) == 0) {
                    tips = data.rows[i].DetailName + "的应收金额不能为0";
                    break;
                }
                if (data.rows[i].LimitTime == "") {
                    tips = "请选择" + data.rows[i].DetailName + "的缴费截止时间";
                    break;
                }
            }
            if (tips != "") {
                Easy.centerShow("系统消息", tips, 3000);
                return false;
            }
            else {
                $("#sOrderContent").textbox("setValue", JSON.stringify(data));
                $("#sOrderName").textbox("setValue", sName);
                $("#choosesOrderDetail").dialog('close');
            }
        }
        else {
            Easy.centerShow("系统消息", "请添加收费项目", 1000);
        }
    });
}

bindsEnrollMajor = function (studentID) {
    $("#sEnrollMajor").combobox({
        url: "../sOrder/SelsEnrollMajor?StudentID=" + studentID + "&MenuID=" + menuId,
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            $('#SchemeSemester').combobox('clear');
            $('#SchemeSemester').combobox('loadData', '');
            bindsEnrollScheme(data.Id);
        }
    });
}

bindsEnrollScheme = function (sEnrollsProfessionID) {
    $("#sEnrollScheme").combobox({
        url: "../sOrder/SelsEnrollScheme?sEnrollsProfessionID=" + sEnrollsProfessionID,
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            bindSchemeSemester(data.Id);
        }
    });
}

bindSchemeSemester = function (planItemID) {
    $("#SchemeSemester").combobox({
        url: "../sOrder/SelSchemeSemester?planItemID=" + planItemID,
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

bindAddFormEvent();
