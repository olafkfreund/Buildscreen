﻿using System;
using ParkSquare.BuildScreen.Web.Services.AzureDevOps.Dto;

namespace ParkSquare.BuildScreen.Web.Services.AzureDevOps
{
    public class BuildDtoConverter : IBuildDtoConverter
    {
        private readonly IBranchNameConverter _branchNameConverter;
        private readonly IDisplayTransformer _displayTransformer;

        public BuildDtoConverter(IBranchNameConverter branchNameConverter, IDisplayTransformer displayTransformer)
        {
            _branchNameConverter = branchNameConverter ?? throw new ArgumentNullException(nameof(branchNameConverter));
            _displayTransformer = displayTransformer ?? throw new ArgumentNullException(nameof(displayTransformer));
        }

        public Build Convert(BuildDto buildDto, TestResults testResults)
        {
            return new Build
            {
                Id = buildDto.Id,
                BuildReportUrl = buildDto.Links.Web.Href,
                RequestedByName = buildDto.RequestedFor.DisplayName,
                Status = string.IsNullOrEmpty(buildDto.Result) ? buildDto.Status : buildDto.Result,
                TotalNumberOfTests = testResults?.TotalTests ?? 0,
                PassedNumberOfTests = testResults?.PassedTests ?? 0,
                TeamProject = _displayTransformer.Tranform(buildDto.Project.Name),
                BuildDefinition = _displayTransformer.Tranform(buildDto.Definition.Name),
                StartBuildDateTime = buildDto.StartTime,
                FinishBuildDateTime = buildDto.FinishTime,
                RequestedByPictureUrl = string.Empty,
                Branch = ConvertBranchName(buildDto.SourceBranch),
                RepoName = _displayTransformer.Tranform(buildDto.Repository.Name)
            };
        }

        private string ConvertBranchName(string branchName)
        {
            return _branchNameConverter.Convert(branchName);
        }
    }
}
