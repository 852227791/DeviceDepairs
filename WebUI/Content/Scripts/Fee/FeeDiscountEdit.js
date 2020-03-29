
bindlistLoad = function () {
    var itemdata = $("#editItemDetailJson").val();
    var discountdata = $("#editFeeDiscountJson").val();
    var data;
    if (discountdata != "" && discountdata != null && discountdata != "[]") {
        data = discountdata;
    }
    else {
        data = itemdata;
    }
    $("#feediscountList").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        columns: [[
            { field: "ItemDetailID", hidden: true },
            { field: "OffsetItem", title: "收费项名称", width: 115, sortable: true },
            { field: "DiscountMoney", title: "优惠金额", width: 100, sortable: false, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
        ]],
        data: JSON.parse(data)
    });
}
bindlistLoad();
//if (FeeID != "0") {
//    $("#feediscountList").datagrid({ data: JSON.parse($("#editFeeDiscountJson").textbox("getValue")) });
//}
