var contact;

bindFormEvent = function () {
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
                setTimeout(function () {
                    $("#city").combobox("setValue", child.pid.toString());
                    setTimeout(function () {
                        $("#district").combobox("setValue", child.id.toString())
                    }, 1);
                }, 1);
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
    $("#nation").combobox({
        url: "../Refe/SelList?RefeTypeID=24",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#modify_tabs").tabs({
        onSelect: function (title, index) {
            if (index === 1) {
                $("#btnSaveModify").hide();
            }
            else {
                $("#btnSaveModify").show();
            }
        }
    });

    if (StudentID != "0") {
        var result = Easy.bindSelectInfo("../StudentInfo/GetStudentInfo", StudentID);
        if (result.IsError === false) {
            var info = JSON.parse(result.Data);
            if (info.StudentInfoID != null && info.StudentInfoID != "") {
                setTimeout(function () {
                    $("#studentInfoId").textbox("setValue", info.StudentInfoID);
                    $("#province").combobox("setValue", info.ProvinceID);
                    $("#city").combobox("select", info.CityID);
                    $("#district").combobox("select", info.DistrictID);
                    $("#nation").combobox("setValue", info.Nation);
                    $("#zip").textbox("setValue", info.Zip);
                    $("#school").textbox("setValue", info.School);
                    $("#studentId").textbox("setValue", StudentID);
                    var row = $("#grid").datagrid("getSelected");
                    $("#address").textbox("setValue", row.Address);
                    $("#qq").textbox("setValue", row.QQ);
                    $("#wechat").textbox("setValue", row.WeChat);
                }, 1);
            }
            else {
                setTimeout(function () {
                    var row = $("#grid").datagrid("getSelected");
                    $("#address").textbox("setValue", row.Address);
                    $("#qq").textbox("setValue", row.QQ);
                    $("#wechat").textbox("setValue", row.WeChat);
                    $("#studentId").textbox("setValue", StudentID);
                }, 1)
            }

        }
        else {
            Easy.centerShow("系统消息", result.Message, 2000);
        }
    }

};

initGrid = function () {
    contact = $("#contactgrid").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        saveUrl: "../StudentContact/GetStudentContactEdit",
        updateUrl: "../StudentContact/GetStudentContactEdit",
        queryParams: { StudentID: StudentID },//异步查询的参数
        columns: [[
            { field: "StudentContactID", idField: "StudentContactID", hidden: true, width: 150 },
            { field: "StudentID", title: "SudentID", hidden: true, width: 150 },
            { field: "Name", title: "姓名", width: 150, sortable: true, editor: { type: "validatebox", options: { required: true, validType: 'length[1,32]' } } },
            { field: "Tel", title: "联系电话", width: 150, sortable: true, editor: { type: "validatebox", options: { required: true, validType: 'length[1,32]' } } }
        ]],
        url: "../StudentContact/GetStudentContactList",
        toolbar: [{
            iconCls: 'icon-add',
            text: "添加",
            handler: function () {
                var tempData = contact.edatagrid('getData');
                contact.edatagrid('addRow', {
                    index: tempData.rows.length,
                    row: {
                        StudentID: StudentID
                    }
                });

            }
        }, '-', {
            iconCls: 'icon-save',
            text: "保存",
            handler: function () {
                contact.edatagrid('saveRow');
            }
        }]
    });
}
bindFormEvent();
initGrid();