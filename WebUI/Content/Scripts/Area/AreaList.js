var selectRow;
firstFunction = function () {
    //加载表格
    initTable({});


    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editArea", "#fSave", "../Area/GetAreaEdit", "#btnSave", "1", "#grid", menuId, "1", "#AreaID");
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
            { field: "AreaID", hidden: true },
            { field: "ParentName", title: "父级", width: 180, sortable: true },
            { field: "Name", title: "名称", width: 180, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        url: "../Area/GetAreaList", sortName: "AreaID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "停用") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = {//得到用户输入的参数
            Name: $("#txtName").val()
        }
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({});
    });
}



//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        selectRow = null;
        $("#btnSave").show();//显示保存按钮
        Easy.OpenDialogEvent("#editArea", "编辑区域信息", 300, 280, "../Area/AreaEdit", "#editArea-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "区域信息")) {
            selectRow = rows[0];
            Easy.OpenDialogEvent("#editArea", "编辑区域信息", 300, 280, "../Area/AreaEdit", "#editArea-buttons");
        }
    }
}


//启用
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
}

//绑定更新状态方法
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "区域信息")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Area/GetUpdateStatus", rows[0].AreaID, "1", "#grid");
    }
}
createAreaJosn = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "createAreaJosn")) {
        var result = Easy.bindSelectInfo("../Area/GetProvinceFile", "");
        Easy.centerShow("系统消息", result.Message,2000);
    }
}

