﻿using System.Linq.Expressions;
using TrickingLibirary.Domain.Entities.Modertion;

namespace TrickingLibirary.Api.ViewModels;

public static class ModerationItemViewModels
{
    public static readonly Func<ModerationItem, object> Create = Projection.Compile();
    public static Expression<Func<ModerationItem, object>> Projection =>
        modItem => new
        {
            modItem.Id,
            modItem.Current,
            modItem.Target,
            modItem.Type,
            Comments = modItem.Comments.AsQueryable().Select(CommentViewModel.Projection).ToList(),
            Reviews = modItem.Reviews.AsQueryable().Select(ReviewViewModel.Projection).ToList(),
        };
}
