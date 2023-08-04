﻿using Data.Entities.Products.Courses;
using Services.Dto.AdminPanel.Course.CourseEpisode;
using Services.DTOs.AdminPanel.Course.CourseEpisode;

namespace Services.Contracts.Interfaces;

public interface ICourseEpisodeServices
{
    Task<IEnumerable<ShowEpisodesForSeasonDto>> GetEpisodesForSeasonAsync(int seasonId, CancellationToken cancellationToken = default, bool? isDeleted = null);
    Task<bool> IsEpisodeExistAsync(string episodeTitle, int seasonId, CancellationToken cancellationToken = default);
    Task<bool> IsEpisodeFileExistAsync(string fileName, CancellationToken cancellationToken = default);
    Task<EditEpisodeDto> GetEpisodeForEditAsync(int episodeId, CancellationToken cancellationToken = default);
    Task<CourseEpisode> GetEpisodeByIdAsync(int episodeId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);


    Task AddEpisodeAsync(CourseEpisode courseEpisode, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateCourseEpisodeAsync(CourseEpisode courseEpisode, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}