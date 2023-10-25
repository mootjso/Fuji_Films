//public static class ReservationHandler
//{
//    public static Show? SelectMovieFromSchedule()
//    {
//        bool inMenu = true;
//        while (inMenu)
//        {
//            List<string> dates = GetDatesInFuture();
//            if (dates.Count == 0)
//            {
//                List<string> menuOption = new() { "Back" };
//                Menu.Start("There are no movies scheduled at the moment, please come back later.\n", menuOption);

//                return null;
//            }

//            // Date selection
//            dates.Add("Back");
//            int index = Menu.Start("Select a date to see the movies for that day\n", dates);
//            if (index == dates.Count || index == dates.Count - 1)
//                break;

//            string dateString = dates[index];
//            DateTime date = DateTime.Parse(dateString);

//            List<Show> moviesForDate = ShowHandler.GetShowsByDate(date);

//            // Create list of formatted strings to display to the user
//            List<string> movieMenuString = CreateListMovieStrings(moviesForDate);

//            index = Menu.Start($"Date: {dateString}\n", movieMenuString);
//            if (index == movieMenuString.Count || index == movieMenuString.Count - 1)
//                continue;

//            return moviesForDate[index];
//        }
//        return null;
//    }
//}