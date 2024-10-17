
# NFluent.Json


[NFluent](http://www.n-fluent.net/) Extensions for [JsonElement](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement).



## Check a JsonElement value

```c#
Check.That(jsonElement1).HasNullValue();
Check.That(jsonElement2).HasStringValue("foo");
Check.That(jsonElement3).HasIntValue(42);
Check.That(jsonElement4).HasLongValue(2147483648);
Check.That(jsonElement5).HasBoolValue(true);
Check.That(jsonElement6).HasTrueValue();
Check.That(jsonElement7).HasFalseValue();
Check.That(jsonElement7).HasGuidValue();
Check.That(jsonElement9).HasArrayValue(new[]{1, 2});
Check.That(jsonElement10).HasArrayValueEquivalentTo(new[]{2, 1});
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
Check.That(jsonElement).HasBoolProperty("boolProperty", expectedValue: true);
Check.That(jsonElement).HasIntProperty("intProperty", 42);
Check.That(jsonElement).HasLongProperty("longProperty", 2147483648);
Check.That(jsonElement).HasNullProperty("nullProperty");
Check.That(jsonElement).HasGuidProperty("nullProperty", Guid.NewGuid());
Check.That(jsonElement).HasArrayProperty("arrayProperty", new[] { 1, 2 });
Check.That(jsonElement).HasArrayPropertyEquivalentTo("arrayProperty", new[] { 2, 1 });
```
