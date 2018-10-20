(function ($)
{

	$.fn.loginView = function (options)
	{

		// This is the easiest way to have default options.
		var settings = $.extend({
			signIn: undefined,
			signOut: undefined,
			get: undefined,
			homePage: undefined,
			connector: undefined,
			userProfile: undefined,
			adminPanel: undefined
		}, options);

		var container = $(this);






		container.get = function ()
		{
			var result = null;
			container.ax({
				url: settings.get,
				async: false,
				success: function (data)
				{
					if(data !== null && data.id !== undefined)
						result = data;

				}
			});
			return result;
		};

		container.isLogedIn = function ()
		{
			var user = container.get();
			if(user === null || !user)
				return false;
			else return true;
		};

		container.load = function ()
		{
			container.html("");
			var login = container.append("<div></div>").find("div").addClass("loginView").addClass("row");
			login = login.append('<div class="account-wall">').find("div");
			login.append('<img class="profile-img" src="https://lh5.googleusercontent.com/-b0-k99FZlyE/AAAAAAAAAAI/AAAAAAAAAAA/eu7opA4byxI/photo.jpg?sz=120" alt="" >');
			login.append('<input type="text" class="form-control email" placeholder="Email" required autofocus>');
			login.append('<input type="password" class="form-control password" placeholder="Password" required>');
			login.append('<button class="btn btn-lg btn-primary btn-block" type="submit"> Sign in</button>');
			login.append('<label class="checkbox pull-left"><input type="checkbox" class="rememberme" value="remember-me">Remember me</label>');

			container.find(".btn").click(function ()
			{
				container.login();
			});
			return container;
		};

		container.loadLogedInView = function ()
		{
			container.html("");
			var user = container.get();
			$(settings.connector).html(user.person.fullName);
			//container.append("<span>" + user.email + " </span>");
		};

		container.login = function ()
		{
			var email = container.find(".email").val();
			var password = container.find(".password").val();
			var rememberme = container.find(".rememberme").is(":checked");
			container.ax({
				url: settings.signIn,
				data: JSON.stringify({ email: email, password: password, rememberMe: rememberme }),
				async: false,
				success: function (data)
				{
					if(data === null)
					{
						container.find(".email").parent().append("<span class='error'> Email or Password dose not match </span>");
					} else
					{
						window.location = settings.homePage;
					}
				}
			});
		};

		if(container.isLogedIn())
		{
			//container.load();
			container.loadLogedInView();
			if(settings.connector !== undefined)
			{
				var user = container.get();
				var datasource = [{
					text: $("<span/>", { html: "Logout", "class": "fa fa-power-off" }),
					click: function ()
					{
						$("body").ax({
							url: settings.signOut,
							async: false,
							success: function (data)
							{
								window.location = settings.signIn;
							}
						});
					}
				}, {
					text: $("<span/>", { html: "Profile", "class": "fa fa-user" }),
					click: function ()
					{
						window.location.href = settings.userProfile + "?Id=" + user.id;
					}
				}
				];
				if(user.role.roleType === "Administrator")
				{
					datasource.push({
						text: $("<span/>", { html: "Admin Panel", "class": "fa fa-user-secret" }),
						click: function ()
						{
							window.location = settings.adminPanel;
						}
					});
				}
				$(settings.connector).contextMenu({
					action: "left",
					dataSource: datasource
				});
			}
		}
		else
		{
			$(settings.connector).click(function ()
			{
				window.location = settings.signIn;
			});
		}




		return container;
	};
}(jQuery));