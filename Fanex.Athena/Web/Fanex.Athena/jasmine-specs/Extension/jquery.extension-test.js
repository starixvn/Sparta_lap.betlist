describe("JQuery Extensions", function () {
    describe("$.fn.checkboxgroup()", function () {
        function createElements() {
            for (var i = 0; i < 10; i++) {
                affix("input[type=checkbox]");
            }
        }

        var $allCheckboxes, $firstCheckbox, $theOtherCheckboxes
        beforeEach(function () {
            createElements();

            $allCheckboxes = $("input[type=checkbox]");
            $firstCheckbox = $allCheckboxes.first();
            $theOtherCheckboxes = $allCheckboxes.slice(1);
        });

        describe("First item is check all", function () {
            it("First item should be checked when the other were checked", function () {               
                $theOtherCheckboxes.prop("checked", true);

                $allCheckboxes.checkboxgroup();

                expect($firstCheckbox.prop("checked")).toBe(true);
            });

            it("First item should be unchecked when the other have unchecked checkboxes", function () {
                $theOtherCheckboxes.prop("checked", true);
                $theOtherCheckboxes.first().prop("checked", false);

                $allCheckboxes.checkboxgroup();

                expect($firstCheckbox.prop("checked")).toBe(false);
            });
        });

        describe("events", function () {
            it("The other checkboxes should be checked when first checkbox fired change event", function () {
                $allCheckboxes.checkboxgroup();

                $firstCheckbox.prop("checked", true);
                $firstCheckbox.change();

                var lengthChecked = $theOtherCheckboxes.filter(":checked").length;

                expect(lengthChecked).toBe($theOtherCheckboxes.length);
            });

            describe("Change event of the other checkboxes", function () {
                it("First checkbox should be checked when the other were checked all", function () {
                    $allCheckboxes.checkboxgroup();
                    $theOtherCheckboxes.prop("checked", true);

                    $theOtherCheckboxes.first().change();

                    expect($firstCheckbox.prop("checked")).toBe(true);
                });

                it("First checkbox should be unchecked when one of the other were unchecked", function () {
                    $allCheckboxes.checkboxgroup();
                    $theOtherCheckboxes.prop("checked", true);
                    $theOtherCheckboxes.last().prop("checked", false);

                    $theOtherCheckboxes.first().change();

                    expect($firstCheckbox.prop("checked")).toBe(false);
                });
            });
        });
    });

    describe("$.fn.showHide(isShow)", function () {
        var $element;
        beforeEach(function () {
            $element = affix("input");
        });

        it("Element should be showed when 'isShow' parameter equal true", function () {
            $element.showHide(true);

            expect($element).toBeVisible();
        });

        it("Element should be hid when 'isShow' parameter equal false", function () {
            $element.showHide(false);

            expect($element).not.toBeVisible();
        });
    });

    describe("$.format", function () {
        it("$.format should return correct text", function () {
            var pattern = "This is a case which is {0} by {1}";
            var expected = "This is a case which is tested by Walter";

            var result = $.format(pattern, "tested", "Walter");

            expect(result).toBe(expected);
        });
    });

    describe("$.fn.serializeObject()", function () {
        var $form;
        beforeEach(function () {
            $form = affix("form");
            $form.append("<input name='Id' value=1 />");
            $form.append("<input name='Name' value=SB />");
        });

        it("$.fn.serializeObject() should be return correct object", function () {
            var expected = { Id: "1", Name: "SB" };

            var result = $form.serializeObject();

            expect(result).toEqual(expected);
        });
    });

    describe("$.fn.tableRowSpan(colIndexs, effectToIndex)", function () {
        var $table;
        function createElements() {
            $table = affix("table");
            $table.append('<tr><td>1</td><td>SB</td><td>USD</td></tr>');
            $table.append('<tr><td>1</td><td>SB</td><td>USD</td></tr>');
            $table.append('<tr><td>1</td><td>SB</td><td>USD</td></tr>');
        }

        beforeEach(function () {
            createElements();
        });

        it("td tags should have correct colspan", function () {
            $table.tableRowSpan([0, 2]);

            var firstRow = $table.find("tr").first();

            expect(firstRow.find("td").eq(0).attr("rowspan")).toBe("3");
            expect(firstRow.find("td").eq(2).attr("rowspan")).toBe("3");
        });
    });

    describe("$.fn.numbericTextBox(min, max)", function () {
        var $input;
        beforeEach(function () {
            $input = affix("input");
        });

        it("event.preventDefault() should be called when 'input' fired keydown event with value 22", function () {
            var event = $.Event("keydown");
            event.keyCode = 22;

            spyOn(event, "preventDefault");

            $input.numbericTextBox(0, 2000);
            $input.trigger(event);

            expect(event.preventDefault).toHaveBeenCalled();
        });

        it("input value should be formated correctly", function () {
            var event = $.Event("keyup");
            $input.val("11384");

            $input.numbericTextBox(0, 999999);
            $input.trigger(event);

            expect($input.val()).toBe("11,384");
        });
    });
});