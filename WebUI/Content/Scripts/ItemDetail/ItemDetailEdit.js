//绑定表单数据
bindFormEvent = function () {
    $("#DetailID").combotree({
        url: "../Detail/GetDetailCommonbox",
        animate: true,
        lines: true,
        panelWidth: 300,
        editable: true,
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#DetailID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#DetailID").combotree("setValue", nodeTree.id);
            }
        },
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#DetailID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 2000);
            }
        }
    });
    $("#SortID").combobox({
        url: "../Refe/SelList?RefeTypeID=5",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        editable: false,
        value: 1
    });
    $("#IsReport1").combobox({
        url: "../Refe/SelList?RefeTypeID=13",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        value: 2
    });
    $("#IsShow1").combobox({
        url: "../Refe/SelList?RefeTypeID=13",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        value: 1
    });
    $("#IsGive").combobox({
        url: "../Refe/SelList?RefeTypeID=15",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        editable: false,
        value: 1
    });
}

//绑定验证事件


//绑定显示部门信息的方法
bindSelectInfo = function (itemDetailId) {
    if (itemDetailId != "0") {
        $("#DetailID").combotree({
            readonly: true
        });
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../ItemDetail/SelectItemDetail", itemDetailId);
            var i = JSON.parse(result.Message)[0];
            $("#ItemID1").textbox("setValue", i.ItemID);
            $("#ItemDetailID").textbox("setValue", i.ItemDetailID);
            $("#Money").numberspinner("setValue", i.Money);
            $("#DetailID").combotree("setValue", i.DetailID);
            $("#SortID").combobox("setValue", parseInt(i.Sort));
            $("#IsGive").combobox("setValue", i.IsGive);
            $("#IsReport1").combobox("setValue", i.IsReport);
            $("#IsShow1").combobox("setValue", i.IsShow);
            $("#QueueID").numberspinner("setValue", i.Queue);
            $("#Remark1").textbox("setValue", i.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
    else {
        $("#ItemID1").val(ItemID);
    }
}

queryHandler = function (searchText, event) {
    $('#DetailID').combotree('tree').tree("search", searchText);
}

bindFormEvent();//加载表单数据

bindSelectInfo(ItemDetailID);//显示部门信息
