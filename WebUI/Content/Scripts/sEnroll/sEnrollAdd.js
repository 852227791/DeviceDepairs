chooseSchame = "1";
var isShowRadio = false;
bindFormEvent = function () {
    $("#repeatSign").hide();

    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "goon")) {
        $("#continue").attr("disabled", "disabled");
    }
    else {
        $("#continue").attr("disabled", false);
    }

    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "repeat")) {
        $("#repeat").attr("disabled", "disabled");
    }
    else {
        $("#repeat").attr("disabled", false);
    }
    queryHandler = function (searchText, event) {
        $('#search').combotree('tree').tree("search", searchText);
    }
    $("#search").combotree({
        valueField: 'id',
        textField: 'text',
        method: 'get',
        url: '../Content/Json/area.json',
        panelHeight: 300,
        panelWidth: 300,
        onSelect: function (node) {
            var tree = $('#search').combotree('tree');
            var level = CheckLevel(node, node, tree);
        },
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#search').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#search").combotree("setValue", nodeTree.id);
            }
        }

    });

    CheckLevel = function (child, node, tree) {
        if (node.pid === "0") {
            $("#province").combobox("setValue", node.id.toString());

            var children = tree.tree('getChildren', child.target);

            if (child.pid === node.id) {
                $("#city").combobox("setValue", child.id.toString());
                $("#district").combobox("setValue", "");
            }
            else if (child.pid === "0") {
                $("#city").combobox("setValue", "");
            }
            else {
                $("#city").combobox("setValue", child.pid.toString());
                $("#district").combobox("setValue", child.id.toString());
            }

        }
        else {
            var parentNode = tree.tree('find', node.pid);
            CheckLevel(child, parentNode, tree);
        }
    };


    $("#province").combobox({
        valueField: 'id',
        textField: 'text',
        method: 'get',
        url: '../Content/Json/province.json',
        onChange: function (newValue, oldValue) {
            $("#city").combobox("clear");
            $.get('../Content/Json/city.json',
                function (data) {
                    var tempData = [];
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].pid === newValue) {
                            tempData.push(data[i]);
                        }
                    }
                    $("#city").combobox("loadData", tempData);

                }, 'json');
        }

    });

    $("#city").combobox({
        onChange: function (newValue, oldValue) {
            $("#district").combobox("clear");
            $.get('../Content/Json/district.json',
               function (data) {
                   var tempData = [];
                   for (var i = 0; i < data.length; i++) {
                       if (data[i].pid === newValue) {
                           tempData.push(data[i]);
                       }
                   }
                   $("#district").combobox("loadData", tempData);

               }, 'json');
        }
    });

    var result1 = Easy.bindSelectInfo("../Common/GetMonthNow", "");
    var result2 = Easy.bindSelectInfo("../Common/GetDateNow", "");
    if (defaultDeptID != "") {
        $("#senrollDeptId").combobox({
            url: "../DeptArea/GetDeptAreaCombobox",
            queryParams: { DeptID: defaultDeptID },
            valueField: "id",
            textField: "name",
            panelHeight: 120,
            multiple: false,
            editable: false
        });
        loadMajor(defaultDeptID, result2.Message.split('-')[0], result1.Data);
    }
    $("#deptId").combotree({
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
                $('#deptId').combotree('clear');
                $('#professionId').combobox('clear');
                $('#professionId').combobox('loadData', '');
                $('#scheme').textbox('setValue', '');
                $('#schemeName').textbox('setValue', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var year = $("#year").combobox("getValue");
                var month = $("#month").combobox("getValue");
                if (year != "" && month != "") {
                    loadMajor(node.id, year, month);
                    $('#scheme').textbox('setValue', '');
                    $('#schemeName').textbox('setValue', '');
                }
                $("#senrollDeptId").combobox({
                    url: "../DeptArea/GetDeptAreaCombobox",
                    queryParams: { DeptID: node.id },
                    valueField: "id",
                    textField: "name",
                    panelHeight: 120,
                    multiple: false,
                    editable: false
                });
            }
        }
    });

    $("#year").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=Yes&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#deptId").combotree("getValue");
            var month = $("#month").combobox("getValue");
            if (dept != "" && month != "") {
                loadMajor(dept, data.Id, month);
                $('#scheme').textbox('setValue', '');
                $('#schemeName').textbox('setValue', '');
            }
        }
    });


    $("#month").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        value: result1.Data,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#deptId").combotree("getValue");
            var year = $("#year").combobox("getValue");
            if (dept != "" && year != "") {
                loadMajor(dept, year, data.Value);
                $('#scheme').textbox('setValue', '');
                $('#schemeName').textbox('setValue', '');
            }
        }
    });

    $("#enrollLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onChange: function (newValue, oldValue) {
            $('#scheme').textbox('setValue', '');
            $('#schemeName').textbox('setValue', '');
        }
    });
    $("#sex").combobox({
        url: "../Refe/SelList?RefeTypeID=3",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
    $("#nation").combobox({
        url: "../Refe/SelList?RefeTypeID=24",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false,
        value: 1
    });
    $("#enrollTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false,
        readonly: true
    });


    $("#btnchooseScheme").click(function () {
        var major = $("#professionId").combotree("getValue");
        var year = $("#year").combobox("getValue");
        var month = $("#month").combobox("getValue");
        var level = $("#enrollLevel").combobox("getValue")
        if (year == "") {
            Easy.centerShow("系统消息", "请选择年份", 3000);
            return false;
        }
        if (month == "") {
            Easy.centerShow("系统消息", "请选择月份", 3000);
            return false;
        }
        if (level === "") {
            Easy.centerShow("系统消息", "请选择学习层次", 3000);
            return false;
        }
        if (major == "") {
            Easy.centerShow("系统消息", "请选择专业", 3000);
            return false;
        }
        enrollLevel = level;
        sEnrollMajor = $("#professionId").combotree("getValue");
        sEnrollYear = $("#year").combobox("getValue");
        sEnrollMonth = $("#month").combobox("getValue");
        sEnrollSchemeTempData = $("#scheme").textbox("getValue");
        Easy.OpenDialogEvent("#choosesFeeScheme", "选择缴费方案", 800, 600, "../sEnroll/ChooseSchemeList", "#choosesFeeScheme-buttons");
    });

    $("#btnSavesFeeScheme").click(function () {
        var data = $('#chooseschemegrid2').datagrid("getData");
        var enrollLevel = $("#enrollLevel").combobox("getValue");
        var flag = true;
        if (enrollLevel == "1" || enrollLevel == "2" || enrollLevel == "3" || enrollLevel == "7") {
            if (data.rows.length != 1) {
                flag = false;
            }
        }
        else if (enrollLevel == "4" || enrollLevel == "5") {
            if (data.rows.length != 2) {
                flag = false;
            }
        }
        else if (enrollLevel == "6") {
            if (data.rows.length != 3) {
                flag = false;
            }
        }
        if (!flag) {
            Easy.centerShow("系统消息", "学习层次和方案数量不匹配", 2000);
            return false;
        }
        if (data.rows.length > 0) {
            var sName = "";
            for (var i = 0; i < data.rows.length; i++) {
                sName += data.rows[i].Name + ",";
            }
            $("#scheme").textbox("setValue", JSON.stringify(data.rows));
            $("#schemeName").textbox("setValue", sName.substr(0, sName.length - 1));
        }
        else {
            $("#scheme").textbox("setValue", "");
            $("#schemeName").textbox("setValue", "");
        }
        $("#choosesFeeScheme").dialog('close');
    });

    setTimeout(function () {
        $('#name').textbox({
            onChange: function (newValue, oldValue) {
                var idCard = $("#idCard").textbox("getValue");
                if (idCard != "" && newValue != "") {
                    bindValidateStudent(newValue, idCard);
                }
                if (newValue === "") {
                    $("#repeatSign").hide();
                    isShowRadio = false;
                    bingSetNull();
                }
            }
        });
        $('#idCard').textbox({
            onChange: function (newValue, oldValue) {
                var name = $("#name").textbox("getValue");
                if (newValue === "") {
                    $("#repeatSign").hide();
                    isShowRadio = false;
                    bingSetNull();
                }
                if (name != "" && newValue != "") {
                    bindValidateStudent(name, newValue);
                }

                //预报名可以不用填身份证号
                var isRepeat = $("input[name='isRepeat']:checked").val();
                if (isRepeat === "2") {
                    if (newValue === "" || newValue === null) {
                        $("#idCard").textbox("disableValidation");
                    }
                    else {
                        $("#idCard").textbox("enableValidation");
                    }
                }
            }
        });
        $('#province').combobox('setValue', "23");
    }, 1);

    $("input[name='isRepeat']").bind("click", function () {
        if ($(this).val() === "2") {
            $("#idCard").textbox("disableValidation");
            var idCard = $("#idCard").textbox("getValue");
            if (idCard != "" && idCard != null) {
                $("#idCard").textbox("enableValidation");
            }
        }
        else {
            $("#idCard").textbox("enableValidation");
        }
    });
}

loadMajor = function (dept, year, month) {
    $("#professionId").combobox({
        url: "../sEnroll/SelMajor",
        queryParams: { DeptID: dept, Year: year, Month: month },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (record) {
            $('#scheme').textbox('setValue', '');
            $('#schemeName').textbox('setValue', '');
            loadClassCombobox(record.Id);
        }
    });
}

loadClassCombobox = function (sProfessionId) {
    $("#classId").combobox({
        url: "../Class/ClassComboboxBysProfessionID",
        queryParams: { sProfessionID: sProfessionId },
        valueField: "Value",
        textField: "Text",
        multiple: false
    });
}

loadEditShceme = function (editsEnrollsProfessionID) {
    var result = Easy.bindSelectInfo("../sEnroll/GetEditShceme", sEnrollsProfessionID);
    $("#schemeName").textbox("setValue", result.Message);
}
bindFormEvent();

bindValidateStudent = function (name, idCard) {

    var result = Easy.bindSelectInfomation("../Student/GetStudent", { Name: name, IDCard: idCard });
    if (result.Message === "yes") {
        $("#repeatSign").hide();
        isShowRadio = false;
    }
    if (result.IsError === false) {
        if (result.Data != "" && result.Data != null) {
            var stu = JSON.parse(result.Data)[0];
            $("#sex").combobox("setValue", stu.Sex);
            $("#mobile").textbox("setValue", stu.Mobile);
            $("#nation").combobox("setValue", stu.Nation);
            $("#address").textbox("setValue", stu.Address);
            $("#parentName").textbox("setValue", stu.parentName);
            $("#tel").textbox("setValue", stu.parentTel);
            $("#province").combobox("setValue", stu.ProvinceID);
            $("#city").combobox("setValue", stu.CityID);
            $("#school").textbox("setValue", stu.School);
            $("#zip").textbox("setValue", stu.Zip);
            $("#qq").textbox("setValue", stu.QQ);
            $("#weChat").textbox("setValue", stu.WeChat);
            $("#studentInfoId").textbox("setValue", stu.studentInfoId);
            $("#studentContactId").textbox("setValue", stu.studentContactId);
            $("#studentId").textbox("setValue", stu.StudentID);
            var result = Easy.bindSelectInfo("../sEnroll/CheckStudentIsEnroll", stu.StudentID);
            if (result.Data) {
                //已生成过学号
                $("#repeatSign").show();
                isShowRadio = true;
            }
            else {
                $("#repeatSign").hide();
                isShowRadio = false;
            }
        }
        else {
            bingSetNull();
        }

    }
    else {
        $("#repeatSign").hide();
        isShowRadio = false;
        bingSetNull();
        Easy.centerShow("系统消息", result.Message, 2000);

    }


};
bingSetNull = function () {

    $("#studentInfoId").textbox("setValue", "");
    $("#studentContactId").textbox("setValue", "");
    $("#studentId").textbox("setValue", "");
    $("#sex").combobox("setValue", "");
    $("#mobile").textbox("setValue", "");
    $("#nation").combobox("setValue", "1");
    $("#address").textbox("setValue", "");
    $("#parentName").textbox("setValue", "");
    $("#tel").textbox("setValue", "");
    $("#province").combobox("setValue", "23");
    $("#city").combobox("setValue", "");
    $("#school").textbox("setValue", "");
    $("#zip").textbox("setValue", "");
    $("#qq").textbox("setValue", "");
    $("#weChat").textbox("setValue", "");
    var isRepeat = $("input[name='isRepeat']:checked").val();
    if (!isShowRadio && (isRepeat === "3" || isRepeat === "4" || isRepeat === "5")) {

        $("input[name='isRepeat']").get(0).checked = true;
    }
    else {

        $("input[name=isRepeat][value=´" + isRepeat + "´]").attr("checked", true);
    }
    $("#repeatSign").hide();
}