initTableProveChoose = function (queryData) {
    $("#gridProveChoose").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ProveID", hidden: true },
            { field: "DeptName", title: "校区", width: 140, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Mobile", title: "手机号", width: 100, sortable: true },
            { field: "ItemName", title: "证书名称", width: 140, sortable: true },
            { field: "EnrollTime", title: "报名时间", width: 130, sortable: true },
            { field: "Status", title: "状态", width: 90, sortable: true }
        ]],
        url: "../Prove/GetProveList", sortName: "ProveID", sortOrder: "desc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
            }
        }
    });
}

bindSearchClickEvent = function () {
    $("#btnSearchProveChoose").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTableProveChoose(queryData);//将值传递给
    });
    $("#btnResetProveChoose").click(function () {
        $("#fSearchProveChoose").form("reset");//重置表单
        initTable({ MenuID: menuId, treeDept: DeptID });
    });
}
rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        treeDept: DeptID,
        MenuID: menuId,
        txtName: $("#txtNameChoose").val(),
        txtIDCard: $("#txtIDCardChoose").val(),
        txtMobile: $("#txtMobileChoose").val(),
    }
    return queryData;
}

initTableProveChoose({ treeDept: DeptID, MenuID: menuId });
bindSearchClickEvent();
rerurnQueryData();