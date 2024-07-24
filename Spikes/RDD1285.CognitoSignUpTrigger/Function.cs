using Amazon.Lambda.CognitoEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RDD1285.CognitoSignUpTrigger;

public class Function
{

    public CognitoPreSignupEvent FunctionHandler(CognitoPreSignupEvent input, ILambdaContext context)
    {
        input.Response = new CognitoPreSignupResponse { AutoConfirmUser = true };

        return input;
    }
}
