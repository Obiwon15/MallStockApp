using System;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface IReportService
    {
        Report GetReportByFunc(Func<Report, bool> func);
        bool GetReportBoolByFunc(Func<Report, bool> func);
        Report CreateReport(TimeFrame timeFrame);
    }   
}