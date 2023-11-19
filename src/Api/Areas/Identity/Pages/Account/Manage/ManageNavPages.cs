using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace CloudDrive.Areas.Identity.Pages.Account.Manage
{
	public static class ManageNavPages
	{
		public static string Index => "Index";

		public static string Email => "Email";

		public static string Saved => "Saved";

		public static string ChangePassword => "ChangePassword";

		public static string DownloadPersonalData => "DownloadPersonalData";

		public static string DeletePersonalData => "DeletePersonalData";

		public static string ExternalLogins => "ExternalLogins";

		public static string PersonalData => "PersonalData";

		public static string TwoFactorAuthentication => "TwoFactorAuthentication";

		public static string Education => "Education";

		public static string Cv => "Cv";

		public static string Certificates => "Certificates";

		public static string Experience => "Experience";

		public static string Applied => "Applied";

		public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

		public static string EducationNavClass(ViewContext viewContext) => PageNavClass(viewContext, Education);

		public static string CvNavClass(ViewContext viewContext) => PageNavClass(viewContext, Cv);

		public static string CertificatesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Certificates);

		public static string ExperienceNavClass(ViewContext viewContext) => PageNavClass(viewContext, Experience);

		public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

		public static string SavedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Saved);

		public static string AppliedNavClass(ViewContext viewContext) => PageNavClass(viewContext, Applied);

		public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

		public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

		public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

		public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

		public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

		public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

		private static string PageNavClass(ViewContext viewContext, string page)
		{
			var activePage = viewContext.ViewData["ActivePage"] as string
				?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
			return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
		}
	}
}
