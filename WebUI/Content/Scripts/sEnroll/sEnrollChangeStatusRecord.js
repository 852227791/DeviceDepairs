
firstFunction = function () {
    //加载表格
    var status = "1";
    if (Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "viewDisable")) {
        status = "1,2";
    }

    initTable({ MenuID: menuId,Status:status });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();
    Easy.bindOpenCloseSearchBoxEvent(90);
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptName", title: "校区", width: 100, sortable: true },
            { field: "StudName", title: "姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 150, sortable: true },
            { field: "Major", title: "专业", width: 150, sortable: true },
            { field: "EnrollNum", title: "学号", width: 150, sortable: true },
            { field: "BeforeEnrollTime", title: "预报名时间", width: 150, sortable: true },
            { field: "UserName", title: "操作人", width: 150, sortable: true },
            { field: "CreateTime", title: "转正报时间", width: 150, sortable: true }

        ]],
        url: "../sEnroll/GetChangeStatusList", sortName: "CreateTime", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "2") {
                return "color:#ff0000;";
            }
        }
    });
}
rerurnQueryData = function () {
    var status = "1";
    if (Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "viewDisable")) {
         status = "1,2";
    }

    var queryData = {//得到用户输入的参数
        Status:status,
        name: $("#txtStuName").textbox("getValue"),
        timeS: $("#txtTimeS").datebox("getValue"),
        timeE: $("#txtTimeE").datebox("getValue"),
        deptId: $("#treeDept").combotree("getValue"),
        name: $("#txtStuName").textbox("getValue"),
        userName: $("#txtUserName").textbox("getValue"),
        idCard: $("#txtIDCard").textbox("getValue"),
        MenuID: menuId
    }
    return queryData;
}
//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
}

//添加
download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../sEnroll/DownChangeStatusList",
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
