var isSelected = 0;
firstFunction = function () {
    bindSerchForm();
    bindsFeeDetailTable();
    bindSearchClickEvent();
}
bindsFeeDetailTable = function () {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
            { field: "FeeUser", title: "收费员", align: "left", sortable: true,width:100    },
            { field: "VoucherNumCount", title: "凭证单数", align: "right", sortable: true, width: 80 },
            { field: "FeeStuCount", title: "实收人数", align: "right", sortable: true, width: 80 },
            { field: "FeeShouldMoney", title: "应收总额", align: "right", sortable: true, width: 120 },
            { field: "FeePaidMoney", title: "实收总额", align: "right", sortable: true, width: 120 },
            { field: "FeeDiscountMoney", title: "优惠金额", align: "right", sortable: true, width: 120 },
            { field: "FeeMoney", title: "现金合计", align: "right", sortable: true, width: 120 },
            { field: "PosMoney", title: "刷卡合计", align: "right", sortable: true, width: 120 },
            { field: "Other", title: "其他合计", align: "right", sortable: true, width: 120 }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
            var VoucherNumCount = 0;
            var FeeStuCount = 0;
            var FeeShouldMoney = 0;
            var FeePaidMoney = 0;
            var FeeDiscountMoney = 0;
            var FeeMoney = 0;
            var PosMoney = 0;
            var Other = 0;
            for (var i = 0; i < data.rows.length; i++) {
                VoucherNumCount = parseFloat(VoucherNumCount) + parseFloat(data.rows[i].VoucherNumCount);
                FeeStuCount = parseFloat(FeeStuCount) + parseFloat(data.rows[i].FeeStuCount);
                FeeShouldMoney = parseFloat(FeeShouldMoney) + parseFloat(data.rows[i].FeeShouldMoney);
                FeePaidMoney = parseFloat(FeePaidMoney) + parseFloat(data.rows[i].FeePaidMoney);
                FeeDiscountMoney = parseFloat(FeeDiscountMoney) + parseFloat(data.rows[i].FeeDiscountMoney);
                FeeMoney = parseFloat(FeeMoney) + parseFloat(data.rows[i].FeeMoney);
                PosMoney = parseFloat(PosMoney) + parseFloat(data.rows[i].PosMoney);
                Other = parseFloat(Other) + parseFloat(data.rows[i].Other);
            }
            $('#grid').datagrid('reloadFooter', [
               {
                   VoucherNumCount: '合计:' + VoucherNumCount, FeeStuCount: '合计:' + FeeStuCount,
                   FeeShouldMoney: '合计:' + parseFloat(FeeShouldMoney).toFixed('2'), FeePaidMoney: '合计:' + parseFloat(FeePaidMoney).toFixed('2'),
                   FeeDiscountMoney: '合计:' + parseFloat(FeeDiscountMoney).toFixed('2'), FeeMoney: '合计:' + parseFloat(FeeMoney).toFixed('2'),
                   PosMoney: '合计:' + parseFloat(PosMoney).toFixed('2'), Other: '合计:' + parseFloat(Other).toFixed('2')
               },
            ]);
        }
    });
}

bindSerchForm = function () {
    $("#treeDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
    
}

queryHandler = function (searchText, event) {
    $('#treeDetail').combotree('tree').tree("search", searchText);
}

download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "收费员收费报表.xls");
    }
};
bindSelect = function (data) {
    $("#grid").datagrid({ url: "../sReport/GetFeeUserReport", queryParams: data, pageNumber: 1 });
}
bindSearchClickEvent = function () {
  
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var data = getSeachData();
        if (data.treeDeptID === "") {
            Easy.centerShow("系统消息", "请选择收费校区", 2000);
            return false;
        }
        bindSelect(data);
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        $("#grid").datagrid({ url: "" });
    });
}
var getSeachData = function () {
    var queueData = {
        MenuID: menuId,
        treeDeptID: $("#treeDeptID").combotree("getValue"),
        txtFeeTimeS: $("#txtFeeTimeS").datebox("getValue"),
        txtFeeTimeE: $("#txtFeeTimeE").datebox("getValue")
    }
    return queueData;
}