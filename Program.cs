using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using SD_Core_Api.Interfaces;
using SD_Core_Api.Jobs;
using SD_Core_Api.Repository;
using SD_Core_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseMemoryStorage()
.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDBConnection"), new SqlServerStorageOptions
{
    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    QueuePollInterval = TimeSpan.Zero,
    UseRecommendedIsolationLevel = true,
    DisableGlobalLocks = true
})
);

var options = new BackgroundJobServerOptions()
{
    Queues = new[] { "queue-a_ap", "queue-b_dn", "queue-c_cn", "queue-d_ocr", "queue-e_nrdc", "queue-f_grn", "queue-g_stk", "queue-h_bp", "queue-i_jrnl" }
};



//builder.Services.AddHangfireServer();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISapPosting, SapPostingRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();

//}


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

new BackgroundJobServer(options);

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = "Admin",
                                PasswordClear = "123456"

                            }
                        }
                    }) },
    AppPath = "/hangfire",
    DefaultRecordsPerPage = 20,
    DisplayStorageConnectionString = true,
    DarkModeEnabled = true,
    DashboardTitle = "Tasks Dashboard"
});


app.UseHangfireDashboard("/hangfire");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new Hangfire.Dashboard.AllowAllConnectionsFilter() }
});

RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_AP", x => x.SAPSDAP(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_DN", x => x.SAPSDDN(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_CN", x => x.SAPSDCN(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_CR", x => x.SAPSDOCR(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_NRDC", x => x.SAPSDNRDC(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_GRN", x => x.SAPSDGRN(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_STK", x => x.SAPSDSTK(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_BP", x => x.SAPSDBP(), "0/30 * * * *");
//RecurringJob.AddOrUpdate<SAPtoSD>("SAP_SD_Post_JRNL", x => x.SAPSDJRNL(), "0/30 * * * *");


app.Run();
