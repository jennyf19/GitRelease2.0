using Octokit;
using Octokit.Helpers;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace GitRlease2
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncReleaseMethod();

            Console.ReadLine();

        }
        #region Methods
        /// <summary>
        /// The AsyncRelease Method is an asyncronous method. 
        /// It creates a plain GitHubClient that includes a User-Agent header.
        /// Authenticated access to the repo is used via personal access token.
        /// The repo is tagged and released with the new of the repo, a name for the release, and
        /// a markdown can be included. newRelease.Draft and newRelease.Prerelease are booleans and
        /// the defaults for both is false. 
        /// </summary>
        public static async void AsyncReleaseMethod()
        { 
           

            //A plain GitHubClient is created. You can use the default string for ProduceHeaderValue or enter your own.
            var client = new GitHubClient(new ProductHeaderValue("DevexGitRelease"));

            //Enter a personal access token for the repo you want to release.
            var tokenAuth = new Credentials("d61ea7300584e6b22656b6892b8499d7884d0ea6");

            client.Credentials = tokenAuth;

            Repository result = await client.Repository.Get("Jennyf19", "KalAcademyRotatedArray");
            Console.WriteLine(result.Id);
            Console.WriteLine(result.GitUrl);
            Console.WriteLine(result.FullName);
            Console.WriteLine(result.StargazersCount);
            Console.WriteLine(result.Url);
            #region Create Tag
            //Enter the name of the repo to be released
            var newRelease = new NewRelease("KalAcademyRotatedArray");

            //Enter the name of the release
            newRelease.Name = "Big Cat Adventure";

            //Include any information you would like to share with the user in the markdown
            newRelease.Body = "This is the markdown";
            
            //The Draft plag is used to indicate when a release should be published
            newRelease.Draft = false;

            //Indicates whether a release is unofficial or preview release
            newRelease.Prerelease = false;

            #endregion

            #region The Release

            ///To create a new release, you must have a corresponding tag in the repository
            var newReleaseResult = await client.Repository.Release.Create(result.Id, newRelease);

            Console.WriteLine("Created release id {0}", newRelease.TagName);
            
            var tagsResult = await client.Repository.GetAllTags(result.Id);
            var tag = tagsResult.FirstOrDefault();

            NewRelease data = newRelease;
            //NewRelease data = new NewRelease("test");
            Release releaseResult;
            try
            {
                var releases = await client.Repository.Release.GetAll(result.Id);

                releaseResult = await client.Repository.Release.Create(result.Id, data);
            }
            catch (NotFoundException e)
            {
                Console.WriteLine("Not found exception");
            }


            Console.WriteLine("All done.");
            Console.ReadLine();
        }
        #endregion
        #endregion
    }
}
