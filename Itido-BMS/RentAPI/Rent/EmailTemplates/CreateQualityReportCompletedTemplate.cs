using System;
using System.IO;
using System.Reflection;
using MimeKit;
using MimeKit.Utils;

namespace Rent.EmailTemplates
{
    public class CreateQualityReportCompletedTemplate
    {
        public static BodyBuilder Build(string header, string body, string path) {
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = @"

<!DOCTYPE html>
<html>
<head>
<title></title>

<meta charset='utf-8'>
<meta name='viewport' content='width=device-width, initial-scale=1'>
<meta http-equiv='X-UA-Compatible' content='IE=edge' />
<style type='text/css'>
    body, table, td, a{-webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;} /* Prevent WebKit and Windows mobile changing default text sizes */
    table, td{mso-table-lspace: 0pt; mso-table-rspace: 0pt;} /* Remove spacing between tables in Outlook 2007 and up */
    img{-ms-interpolation-mode: bicubic;} /* Allow smoother rendering of resized image in Internet Explorer */

    img{border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none;}
    table{border-collapse: collapse !important;}
    body{height: 100% !important; margin: 0 !important; padding: 0 !important; width: 100% !important;}

    a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
    }

    .rating-container {
      display: grid;
      grid-template-columns: auto auto auto auto auto;
      margin-left: 15%;
      margin-right: 15%;
    }

    .star {
      width: 80px;
      height: 80px;
    }

    @media screen and (max-width: 525px) {

        .wrapper {
          width: 100% !important;
            max-width: 100% !important;
        }

        .logo img {
          margin: 0 auto !important;
        }

        .rating-container {
          margin-left: 0px;
          margin-right: 0px;
        }

        .star {
          width: 60px;
          height: 60px;
        }

        .mobile-hide {
          display: none !important;
        }

        .img-max {
          max-width: 100% !important;
          width: 100% !important;
          height: auto !important;
        }

        .responsive-table {
          width: 100% !important;
        }

        .padding {
          padding: 10px 5% 15px 5% !important;
        }

        .padding-meta {
          padding: 30px 5% 0px 5% !important;
          text-align: center;
        }

        .padding-copy {
             padding: 10px 5% 10px 5% !important;
          text-align: center;
        }

        .no-padding {
          padding: 0 !important;
        }

        .section-padding {
          padding: 50px 15px 50px 15px !important;
        }

        .mobile-button-container {
            margin: 0 auto;
            width: 100% !important;
        }

        .mobile-button {
            padding: 15px !important;
            border: 0 !important;
            font-size: 16px !important;
            display: block !important;
        }

        .mobile-button.rating {
            font-size: 25px !important;
        }
    }

    div[style*='margin: 16px 0;'] { margin: 0 !important; }
</style>
</head>
<body style='margin: 0 !important; padding: 0 !important;'>

<div style='display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;'>
    " + body + @"
</div>

<table border='0' cellpadding='0' cellspacing='0' width='100%'>
    <tr>
        <td bgcolor='#ffffff' align='center'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 500px;' class='wrapper'>
                <tr>
                    <td align='center' valign='top' style='padding: 15px 0;' class='logo'>
                        <a href='https://simpapp.dk/' target='_blank'>
                            <img alt='Logo' src='https://rentapp.azurewebsites.net/img/fav/apple-icon-120x120.png' width='80' height='80' style='display: block; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-size: 16px;' border='0'>
                        </a>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td bgcolor='#D8F1FF' align='center' style='padding: 20px 15px 40px 15px;' class='section-padding'>
            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 800px;' class='responsive-table'>
                <tr>
                    <td>
                        <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                            <tr>
                                  <td class='padding' align='center'>
                                    <h1 style='font-size: 32px; font-family: Helvetica, Arial, sans-serif; color: #333333; padding-top: 0px;'>
                                      " + header + @"
                                    </h1>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                        <tr>
                                            <td align='center' style='font-size: 22px; font-family: Helvetica, Arial, sans-serif; color: #333333; padding-top: 10px;' class='padding'>
                                            " + body + @"
                                            </td>
                                        </tr>
                                        <tr>

                                          <td align='center'>
                                              <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                                  <tr>
                                                      <td align='center' style='padding-top: 25px; ' class='padding'>
                                                          <table border='0' cellspacing='0' cellpadding='0' class='mobile-button-container'>
                                                              <tr>
                                                                  <td align='center' style='border-radius: 3px;' bgcolor='#edd523'><a href='https://simpapp.dk/#/" + path +@"' target='_blank' style='font-size: 24px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; border-radius: 3px; padding: 15px 25px; border: 1px solid #edd523; display: inline-block;' class='mobile-button rating'> Giv os din mening </a></td>
                                                              </tr>
                                                          </table>
                                                      </td>
                                                  </tr>
                                              </table>
                                          </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                        <tr>
                                            <td align='center' style='padding: 20px 100px 0 100px; font-size: 14px; line-height: 25px; font-family: Helvetica, Arial, sans-serif; color: #666666;' class='padding'>For mere information gå til hjemmesiden. Der vil du kunne finde information om din rengøringsaftale, kvalitetsrapporter, vigtige dokumenter. Her kan du også se personalet der gør rent hos dig. Du kan altid kontakte os hvis du har nogen spørgsmål.</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align='center'>
                                    <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                        <tr>
                                            <td align='center' style='padding-top: 25px;' class='padding'>
                                                <table border='0' cellspacing='0' cellpadding='0' class='mobile-button-container'>
                                                    <tr>
                                                        <td align='center' style='border-radius: 3px;' bgcolor='#256F9C'><a href='https://simpapp.dk/' target='_blank' style='font-size: 16px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; border-radius: 3px; padding: 15px 25px; border: 1px solid #256F9C; display: inline-block;' class='mobile-button'>Gå til web-appen &rarr;</a></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td bgcolor='#ffffff' align='center' style='padding: 20px 0px;'>
            <table width='100%' border='0' cellspacing='0' cellpadding='0' align='center' style='max-width: 500px;' class='responsive-table'>
                <tr>
                    <td align='center' style='font-size: 12px; line-height: 18px; font-family: Helvetica, Arial, sans-serif; color:#666666;'>
                        Rengøringsselskabet RENT ApS
                        <br>
                        <a href='tel:+4570215500' target='_blank' style='color: #666666; text-decoration: none;'>+45 70 21 55 00</a>
                        <span style='font-family: Arial, sans-serif; font-size: 12px; color: #444444;'>&nbsp;&nbsp;|&nbsp;&nbsp;</span>
                        <a href='mailto:info@rs-rent.dk' target='_blank' style='color: #666666; text-decoration: none;'>info@rs-rent.dk</a>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</body>
</html>


";
            
            return bodyBuilder;
        }
    }
}
