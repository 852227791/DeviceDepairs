var queryDataDefault_Choose = { selStatus: "1", selRoleType: userType };

//加载表格
initTable_Choose = function (queryData) {
    $("#grid-Choose").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "RoleID", hidden: true },
            { field: "Name", title: "角色名", width: 100, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "RoleType", title: "角色分类", width: 70, sortable: true },
            { field: "Description", title: "角色描述", width: 300, sortable: true }
        ]],
        url: "../Role/GetRoleList", sortName: "RoleID", sortOrder: "asc"
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent_Choose = function () {
    $("#btnSearch-Choose").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData_Choose = {//得到用户输入的参数
            txtName: $("#txtName-Choose").val(),
            selRoleType: userType,
            selStatus: "1"
        }
        initTable_Choose(queryData_Choose);//将值传递给
    });
    $("#btnReset-Choose").click(function () {
        $("#fSearch-Choose").form("reset");//重置表单
        initTable_Choose(queryDataDefault_Choose);
    });
}

//加载表格
initTable_Choose(queryDataDefault_Choose);

//加载搜索按钮的点击事件
bindSearchClickEvent_Choose();
