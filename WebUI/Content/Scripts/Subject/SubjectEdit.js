bindFormEvent = function () {
    $("#ParentID").combotree({
        url: '../Subject/GetSubjectList',
        queryParams: { Status: "1" },
        animate: true,
        lines: true,
        onChange: function (newValue, oldValue) {
            $("#ParentID").combotree("validate");
        }
    });
};
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Subject/FormValidateNameIsRepeat";
    row1.id = "#SubjectID";
    row1.type = [];
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Subject/FormValidateParentId";
    row2.id = "#SubjectID";
    row2.type = [{}];//row2.type = [{},{}];
    row2.type[0].typeId = "#ParentID";
    row2.type[0].type = "2";
    data.jsontext.push(row2);
    Easy.checkValue(data);
}

bindSelectInfo = function () {
    if (subjectId != "0") {
        setTimeout(function () {
            var node = $('#subtree').treegrid("getSelected");
            $("#SubjectID").textbox("setValue", node.id);
            $("#Name").textbox("setValue", node.text);
            $("#EnglishName").textbox("setValue", node.EnglishName);
            $("#Remark").textbox("setValue", node.Remark);
            $("#ParentID").combotree("setValue", node.ParentID);
        }, 1);
    }

}
bindCheckEvent();
bindFormEvent();
bindSelectInfo();

