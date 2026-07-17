using learning_middleware;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

//middleware
var pipelineBuilder = new PipelineBuilder()
                        .Use(async (context, next) =>
                        {
                            Console.WriteLine("befor next");
                            await next(context);
                            Console.WriteLine("after next");
                        })
                        .Use(async (context, next) =>
                        {
                            Console.WriteLine("befor codecamp");
                            await next(context);
                            Console.WriteLine("after codecamp");
                        })
                        .Build();

app.Run(context => pipelineBuilder(context));

app.Run();