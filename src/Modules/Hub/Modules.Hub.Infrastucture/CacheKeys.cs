namespace Modules.Hub.Infrastucture;

internal static class CacheKeys
{
	public const string AccountRelatedCachePrefixTemplate = "hub/accounts/{0}";
	public const string AccountCredentialsCacheKeyTemplate = $"{AccountRelatedCachePrefixTemplate}/credentials";
	public const string EmailsCountCacheKeyTemplate = $"{AccountRelatedCachePrefixTemplate}/ emails/count";
	public const string EmailsFoldersCacheKeyTemplate = $"{AccountRelatedCachePrefixTemplate}/folders";
	public const string EmailsFolderCountCacheKeyTemplate = $"{AccountRelatedCachePrefixTemplate}/folders/{{1}}/count";
	public const string EmailsConversationsCacheKeyPrefixTemplate = $"{AccountRelatedCachePrefixTemplate}/folders/{{1}}/conversations";
	public const string EmailsConversationsCacheKeyTemplate = $"{EmailsConversationsCacheKeyPrefixTemplate}/page/{{2}}";
	public const string EmailsCacheKeyTemplate = $"{AccountRelatedCachePrefixTemplate}/conversations/{{1}}/emails";
}
