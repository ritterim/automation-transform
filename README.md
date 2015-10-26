# Automation : Transform

This library is designed to allow the modification of configuration files via the [Microsoft.Web.Xdt](http://www.nuget.org/packages/Microsoft.Web.Xdt/) library. It uses the transform syntax that we all know and love. You can utilize this library **in memory** with no need to access any files, but you can do that as well.

![NuGet Version](https://img.shields.io/nuget/v/RimDev.Automation.Transform.svg)
![NuGet Download Count](https://img.shields.io/nuget/dt/RimDev.Automation.Transform.svg)

## Quick Start

You can install this package via [Nuget](http://nuget.org) under the package id of [RimDev.Automation.Transform](http://www.nuget.org/packages/RimDev.Automation.Transform/).

```
 PM> Install-Package RimDev.Automation.Transform
```

Next step is to have a *source* and a *transform* file ready. Please read the [transformation documentation](http://msdn.microsoft.com/en-us/library/dd465326%28v=vs.110%29.aspx) before attempting to use this library.

When you have your files ready, you can perform a transformation with the following code.

```
using (var transformer = new ConfigurationTransformer()) {
  var result
  = transformer
  .SetSourceFromFile("web.config")
  .SetTransformFromFile("web.debug.config")
  .Apply("web.transformed.config");
}
```

## Transformation Extensions

You get a few methods that allow you to construct transformations programatically.

1. InsertAppSetting
2. ReplaceAppSetting
3. InsertSqlConnectionString
4. ReplaceSqlConnectionString
5. InsertConnectionString
6. ReplaceConnectionString
7. [InsertCustomErrorSetting](http://msdn.microsoft.com/en-us/library/h0hfz6fc(v=vs.100).aspx)
8. [ReplaceCustomErrorSetting](http://msdn.microsoft.com/en-us/library/h0hfz6fc(v=vs.100).aspx)
9. [InsertSmtpSetting](http://msdn.microsoft.com/en-us/library/ms164240%28v=vs.110%29.aspx)
10. [ReplaceSmtpSetting](http://msdn.microsoft.com/en-us/library/ms164240%28v=vs.110%29.aspx)

```
using (var transformer = new ConfigurationTransformer()) {
  transformer
  .SetSourceFromFile("web.config")
  .Transform.InsertAppSetting("hello", "world");

  var result =   transformer.Apply("web.transformed.config");
}
```

The methods utilize the transform syntax, and append it to your transformation config. You can load an existing transform while still appending additional programatic values.

```
using (var transformer = new ConfigurationTransformer()) {
  transformer
  .SetSourceFromFile("web.config")
  .Transform
  .InsertCustomErrorsSetting("ON", "~/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
            })
  .InsertSmtpSetting(smtpDeliveryMethod: SmtpDeliveryMethod.SpecifiedPickupDirectory,
            from: "help@ritterim.com",
            smtpBuilder: smtpBuilder =>
            {
                smtpBuilder.AddSpecifiedPickupDirectory(@"c:\email");
                smtpBuilder.AddNetwork(host: "localhost");
            });

  var result =   transformer.Apply("web.transformed.config");
}
```

The **CustomError** and **Smtp** setting methods allow you to pass an Action to specify the child elements of the respective parent elements. 

## Thanks

Thanks to [Ritter IM](http://ritterim.com) for supporting OSS.
