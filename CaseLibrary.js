//This Library uses a namespace naming strategy to help prevent duplication function names
if (typeof (Cooper) == "undefined") {
    Cooper = {
        __namespace: true
    };
}
// Namespace container for functions in this library.
Cooper.LoanPreOffer= {

    AVPApproveButtonSubmit: function (primaryControl) {

        var formContext = primaryControl; // rename as formContext
        formContext.getAttribute("coo_avpapprovalprovided").setValue(true);
        formContext.data.entity.save();

    },


    __namespace: true
};
