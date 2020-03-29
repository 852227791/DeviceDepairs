//绑定表单数据
bindFormEvent = function () {
    $("#ParentID").combotree({
        url: "../Item/GetItemTree",
        queryParams: { Status: "1", DeptID: DeptID },
        animate: true,
        lines: true,
        panelWidth: 300,
        editable: true,
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#ParentID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#ParentID").combotree("setValue", nodeTree.id);
            }
        },
        onChange: function (newValue, oldValue) {
            $("#Name").textbox("validate");
        }
    });
    $("#IsPlan").combobox({
        url: "../Refe/SelList?RefeTypeID=13",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        value: 2
    });
    $("#PlanLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=25",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false
    });
    $("#IsReport").combobox({
        url: "../Refe/SelList?RefeTypeID=13",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        value: 2
    });
    $("#IsShow").combobox({
        url: "../Refe/SelList?RefeTypeID=13",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        value: 1
    });
    $("#Month").combobox({
        url: "../Refe/SelList?RefeTypeID=16",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false
    });
    
    $("#Sort").combobox({
        url: "../Refe/SelList?RefeTypeID=14",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        onChange: function (newValue, oldValue) {
            $("#Name").textbox("validate");
        }
    });
    $("#Year").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=Yes",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false
    });
}
bindSelectInfo = function (itemId) {
    if (itemId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Item/SelectItem", itemId);
            var i = JSON.parse(result.Message)[0];
            $("#DeptID").textbox("setValue", i.DeptID);
            $("#PlanLevel").combobox("setValue", i.PlanLevel);
            $("#ItemID").textbox("setValue", itemId);
            $("#Name").textbox("setValue", i.Name);
            $("#Sort").combobox("setValue", i.Sort);
            $("#IsPlan").combobox("setValue", i.IsPlan);
            $("#Queue").numberspinner("setValue", i.Queue);
            $("#ParentID").combotree("setValue", i.ParentID);
            $("#EnglishName").textbox("setValue", i.EnglishName);
            $("#Money").numberspinner("setValue", i.Money);
            $("#StartTime").datebox("setValue", Easy.bindSetTimeFormatEvent(i.StartTime));
            $("#EndTime").datebox("setValue", Easy.bindSetTimeFormatEvent(i.EndTime));
            $("#LimitTime").datebox("setValue", Easy.bindSetTimeFormatEvent(i.LimitTime));
            $("#Year").combobox("setValue", i.Year);
            $("#Month").combobox("setValue", i.Month);
            $("#Money").numberspinner("setValue", i.Money);
            $("#Sort").combobox("setValue", i.Sort);
            $("#IsReport").combobox("setValue", i.IsReport);
            $("#IsShow").combobox("setValue", i.IsShow);
            $("#Remark").textbox("setValue", i.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
    else {
        $("#DeptID").val(DeptID);
    }
}

queryHandler = function (searchText, event) {
    $('#ParentID').combotree('tree').tree("search", searchText);
}

bindFormEvent();//加载表单数据

bindSelectInfo(ItemID);//显示部门信息
