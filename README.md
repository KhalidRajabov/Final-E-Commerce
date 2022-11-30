# Final-E-Commerce

TechnoShop is an commercial project where users can post their product they wish to sell. Project has 5 roles and all of them have their specific skills (Higher level role has the ability of lower level roles).

<h3> What I used: </h3>
<ul>
  	<li>C#</li>
  	<li>ASP.NET</li>
  	<li>EF Core 6</li>
  	<li>MVC</li>
  	<li>MsSQL as database</li>
  	<li>Javascript (ES6)</li>
  	<li>JQuery</li>
  	<li>Ajax (GET/POST Requests)</li>
  	<li>Axios (GET/POST Requests)</li>
  	<li>Slick/Splide (Carousel, effects)</li>
  	<li>HTML/CSS</li>
  	<li>SCSS</li>
  	<li>Bootstrap</li>
</ul>


First of all, everyone should be registered. Registration will require a unique email, which will ask you to confirm your registration. We will send a confirmation link to user's email with a link to the site. After clicking the link, it will redirect user to a page where user will be notified if he successfully completed the registration.

<h2> Models and features </h2>
<ol>
	<li>Products</li>
	<li>Blogs</li>
	<li>Subscription</li>
  	<li>Commenting</li>
  	<li>User profile</li>
</ol>



<h4>1. Product</h4>

1.1 Products can be added by anyone with account. When you add a product, it should have picture (at least 1), category, tag and info for required fields. A request will be sent to an admin to check and respond to your pending request. Admins decide whether to allow your prooduct to be posted on site or not. You will get notified by all actions with an email, like your succesful posting, admin response (approval, rejection).
	P.S Product will not be available on the site until it gets "Approved" status by admins.
	
1.2 You can update your product at anytime. You can update anything on your product. When you succesfully updated a product, it gets a status of "Pending", and it will not be visible to any user until an admin confirms it. You will get email updates about process.

1.3 If you have a follower, they will get an email notification about your new product or discounts of your products. 

1.4 Anyone can see products' some details like its rating, how many people wishlisted it, how many times it has been viewed and etc. but only owners or admins can see how many times it has been sold.

1.5 Only those who have bought the product at least for once can rate for it, you can not rate for a product you haven't bought yet...

1.6 Anyone can comment for product, but only those who can have right (member who is the owner of the comment, momderator, admins, superadmin) can delete it. 

1.7 Comments by people without account will be marked as "Not a member" and it can be deleted by anyone that has right to delete a comment on products.

1.8 If people comment for a product which they have rated, their rate (with stars) will be shown on their comments on that product.

1.9 Discounts are shown with discount icon on product cards, and "new" icon will be shown on product card until the product is 3 days old.

<h4>2. Blogs</h4>

2.1 Blogs can only be written by editors or higher level roles (admins and superadmin)

2.2 Posting a new blog automatically sends email notification to site's subscribers. (No notification for updates on blogs)

2.3 Blogs can be commented by anyone and comments can be deleted by comment owners or anyone that has right (moderators, editors, admins, superadmin) to delete it. For blogs, editors can delete any comment on any bloge they want. Editors can not delete any comment on products.

2.4 Comments by people without account will be marked as "Not a member" and it can be deleted by anyone that has right to delete a comment on blogs.



<h2> Roles </h2>
<ol>
	<li>Member</li>
	<li>Moderator</li>
	<li>Editor</li>
  	<li>Admin</li>
  	<li>Super Admin</li>
</ol>

<h3> 1. Member </h3>
<ol>
	<li>Can create products</li>
	<li></li>
	<li></li>
</ol>
