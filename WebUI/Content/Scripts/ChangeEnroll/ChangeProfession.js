firstFunction = function () {
    bindSearchFormEvent();
    bindSearchClickEvent();
    bindGridTableMethod({ MenuID: menuId });
    Easy.bindOpenCloseSearchBoxEvent(90);
};
bindSearchFormEvent = function () {
    $("#selDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 200
    });
}

bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = getSearchData();
        bindGridTableMethod(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        bindGridTableMethod({ MenuID: menuId });
    });
}
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = getSearchData();
        $.ajax({
            type: "post",
            url: "../ChangeEnroll/GetChangeProfessionDownload",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}
bindGridTableMethod = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "StudName", title: "姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "BeSchool", title: "转前校区", width: 100, sortable: true },
            { field: "BeYear", title: "转前年份", width: 60, sortable: true },
            { field: "BeMonth", title: "转前月份", width: 60, sortable: true },
            { field: "BeLevel", title: "转前层次", width: 60, sortable: true },
            { field: "BeMajor", title: "转前专业", width: 100, sortable: true },
            { field: "AfSchool", title: "转后校区", width: 100, sortable: true },
            { field: "AfYear", title: "转后年份", width: 60, sortable: true },
            { field: "AfMonth", title: "转后月份", width: 60, sortable: true },
            { field: "AfLevel", title: "转后层次", width: 60, sortable: true },
            { field: "AfMajor", title: "转后专业", width: 100, sortable: true },
            { field: "ChangeTime", title: "转专业时间", width: 85, sortable: true },
            { field: "ChangeReson", title: "转专原因", width: 200, sortable: true },
             { field: "SortName", title: "记录类型", width: 80, sortable: true }
        ]],
        url: "../ChangeEnroll/GetChangeProfessionList", sortName: "ChangeTime", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

getSearchData = function () {
    var queueData = {
        MenuID:menuId,
        name: $("#txtName").textbox("getValue"),
        idCard: $("#txtIDCard").textbox("getValue"),
        enrollNum: $("#txtEnrollNum").textbox("getValue"),
    };
    return queueData;
};

var selectRow;
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "转专业")) {
            selectRow = rows[0];
            Easy.OpenDialogEvent("#viewChangeProfession", "转专业详细信息", 800, 600, "../ChangeEnroll/ChangeProfessionView", "#viewChangeProfession-buttons");
        }
    }
}
