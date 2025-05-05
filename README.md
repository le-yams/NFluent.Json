
# NFluent.Json


[NFluent](http://www.n-fluent.net/) Extensions for [JsonElement](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement).



## Check a JsonElement value

```c#
Check.That(jsonElement1).HasNullValue();
Check.That(jsonElement2).HasStringValue("foo");
Check.That(jsonElement2).HasStringValue("Foo", StringComparison.OrdinalIgnoreCase);
Check.That(jsonElement3).HasIntValue(42);
Check.That(jsonElement4).HasLongValue(2147483648);
Check.That(jsonElement5).HasBoolValue(true);
Check.That(jsonElement6).IsTrue();
Check.That(jsonElement7).IsFalse();
Check.That(jsonElement7).HasGuidValue(expectedGuid);
Check.That(jsonElement9).HasArrayValue(new[]{1, 2});
Check.That(jsonElement10).HasArrayValueEquivalentTo(new[]{2, 1});
Check.That(jsonElement11).HasSize(3); // works for string and array elements
Check.That(jsonElement12).IsEmpty; // works for string and array elements
```


## Check the presence of a property

```c#
Check.That(jsonElement).HasProperty("foo");
```


## Check the presence and kind of a property

```c#
Check.That(jsonElement).HasProperty("foo", JsonValueKind.String);
```


## Check the presence and value of a property

```c#
Check.That(jsonElement).HasStringProperty("stringProperty", "value");
Check.That(jsonElement).HasStringProperty("stringProperty", "Value", StringComparison.OrdinalIgnoreCase);
Check.That(jsonElement).HasBoolProperty("boolProperty", expectedValue: true);
Check.That(jsonElement).HasIntProperty("intProperty", 42);
Check.That(jsonElement).HasLongProperty("longProperty", 2147483648);
Check.That(jsonElement).HasNullProperty("nullProperty");
Check.That(jsonElement).HasGuidProperty("guidProperty", expectedGuid);
Check.That(jsonElement).HasArrayProperty("arrayProperty", new[] { 1, 2 });
Check.That(jsonElement).HasArrayPropertyEquivalentTo("arrayProperty", new[] { 2, 1 });
```


## Check the size of a property

```c#
Check.That(jsonElement).HasProperty("arrayProperty").HasSize(4);
Check.That(jsonElement).HasProperty("otherArrayProperty").IsEmpty;
Check.That(jsonElement).HasProperty("stringProperty").HasSize(2);
Check.That(jsonElement).HasProperty("otherStringProperty").IsEmpty;
```


## Check element(s) presence using JSON path

```c#
Check.That(jsonElement).HasSingleElementAt("$.foo.bar");
Check.That(jsonElement).HasAtleastOneElementAt("$.foo.array[*]");
Check.That(jsonElement).HasMultipleElementsAt("$.foo.otherArray[*]");
Check.That(jsonElement).HasElementsCountAt("$.foo.thirdArray[*]", 4);
```

> Note that it supports simplified expressions making the `$` and `$.` suffixes optional.
> For example, the following expressions are equivalent:
>  * `$.foo.bar` can be written as `.foo.bar` or `foo.bar`
>  * `$[0].foo` can be written as `[0].foo`

> see also [Find JsonElement inner element with JSON path](#find-jsonelement-inner-element-with-json-path)

## Utility methods

### Find JsonElement inner element with JSON path

The JsonElement extension methods `GetElementAt` and `GetRequiredElementAt` allow you to retrieve a specific inner element using a [JSON path](https://www.rfc-editor.org/rfc/rfc9535.html) expression.

```c#
var foo = jsonElement.GetElementAt("$.foo"); // foo can be null if no element is found
var bar = jsonElement.GetRequiredElementAt("$.bar"); // bar cannot be null (throws if no element is found)
```

> Note that it supports simplified expressions making the `$` and `$.` suffixes optional.
> For example, the following expressions are equivalent:
>  * `$.foo.bar` can be written as `.foo.bar` or `foo.bar`
>  * `$[0].foo` can be written as `[0].foo`
