//绑定表单数据
bindFormEvent = function () {
    $("#ParentID").combotree({
        url: "../Detail/GetDetailTree",
        queryParams: { MenuID: menuId, Status: "1" },
        animate: true,
        lines: true,
        editable: true,
        panelWidth: 300,
        onChange: function (newValue, oldValue) {
            $("#ParentID").combotree("validate");
        },
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#ParentID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#ParentID").combotree("setValue", nodeTree.id);
            }
        }
    });
    $("#SubjectID").combotree({
        url: "../Subject/GetSubjectCombobox",
        valueField: "id",
        textField: "text",
        panelHeight: 120,
        multiple: false,
        editable: true,
        keyHandler: {
            query: queryHandlerSubject
        },
        onHidePanel: function (node) {
            var nodeTree = $('#SubjectID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#SubjectID").combotree("setValue", nodeTree.id);
            }
        }
    });
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Detail/CheckDetailName";
    row1.id = "#DetailID";
    row1.type = [];
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Detail/CheckDetailIDIsParentID";
    row2.id = "#DetailID";
    row2.type = [{}];//row2.type = [{},{}];
    row2.type[0].typeId = "#ParentID";
    row2.type[0].type = "2";
    data.jsontext.push(row2);
    Easy.checkValue(data);
}

//绑定显示收费类别的方法
bindSelectDetailInfo = function (DetailID) {
    if (DetailID != "0") {
        setTimeout(function () {
            var node = $("#detailTree").tree("getSelected");
            $("#DetailID").textbox("setValue", node.id);
            $("#Name").textbox("setValue", node.Name);
            $("#ParentID").combotree("setValue", node.ParentID);
            $("#SubjectID").combotree("setValue", node.SubjectID);
            $("#EnglishName").textbox("setValue", node.EnglishName);
            $("#Remark").textbox("setValue", node.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

queryHandler = function (searchText, event) {
    $('#ParentID').combotree('tree').tree("search", searchText);
}

queryHandlerSubject = function (searchText, event) {
    $('#SubjectID').combotree('tree').tree("search", searchText);
}

bindFormEvent();//加载表单数据

bindSelectDetailInfo(DetailID);//显示部门信息

//验证名称是否重复
bindCheckEvent();