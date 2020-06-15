using System.Collections.Generic;

namespace BA_MobileGPS.Core
{
    public class CountryCode
    {
        public string IsoCode { get; set; }
        public string Name { get; set; }
        public string DialCode { get; set; }
        public string FlagPath { get; set; }

        public string NameSort
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return Name[0].ToString().ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static List<CountryCode> All
        {
            get
            {
                return CountryList;
            }
        }

        public static List<string> AllNames
        {
            get
            {
                List<CountryCode> countryList = CountryList;
                List<string> countryNames = new List<string>();
                foreach (var item in countryList)
                {
                    countryNames.Add(item.Name);
                }

                return countryNames;
            }
        }

        public CountryCode(string IsoCode, string Name, string DialCode, string FlagPath)
        {
            this.IsoCode = IsoCode;
            this.Name = Name;
            this.DialCode = DialCode;
            this.FlagPath = FlagPath;
        }

        public static CountryCode GetCountryByISO(string ISO)
        {
            int foundIndex = -1;
            foreach (var item in CountryList)
            {
                if (item.IsoCode == ISO)
                {
                    foundIndex = CountryList.IndexOf(item);
                }
            }

            if (foundIndex != -1)
            {
                CountryCode foundCountry = CountryList[foundIndex];
                return foundCountry;
            }
            else
            {
                return null;
            }
        }

        private static readonly List<CountryCode> CountryList = new List<CountryCode> {
            new CountryCode("AD", "Andorra", "+376", "flag_ad.png"),
            new CountryCode("AE", "United Arab Emirates", "+971", "flag_ae.png"),
            new CountryCode("AF", "Afghanistan", "+93", "flag_af.png"),
            new CountryCode("AG", "Antigua and Barbuda", "+1", "flag_ag.png"),
            new CountryCode("AI", "Anguilla", "+1", "flag_ai.png"),
            new CountryCode("AL", "Albania", "+355", "flag_al.png"),
            new CountryCode("AM", "Armenia", "+374", "flag_am.png"),
            new CountryCode("AO", "Angola", "+244", "flag_ao.png"),
            new CountryCode("AQ", "Antarctica", "+672", "flag_aq.png"),
            new CountryCode("AR", "Argentina", "+54", "flag_ar.png"),
            new CountryCode("AS", "AmericanSamoa", "+1", "flag_as.png"),
            new CountryCode("AT", "Austria", "+43", "flag_at.png"),
            new CountryCode("AU", "Australia", "+61", "flag_au.png"),
            new CountryCode("AW", "Aruba", "+297", "flag_aw.png"),
            new CountryCode("AX", "Åland Islands", "+358", "flag_ax.png"),
            new CountryCode("AZ", "Azerbaijan", "+994", "flag_az.png"),
            new CountryCode("BA", "Bosnia and Herzegovina", "+387", "flag_ba.png"),
            new CountryCode("BB", "Barbados", "+1", "flag_bb.png"),
            new CountryCode("BD", "Bangladesh", "+880", "flag_bd.png"),
            new CountryCode("BE", "Belgium", "+32", "flag_be.png"),
            new CountryCode("BF", "Burkina Faso", "+226", "flag_bf.png"),
            new CountryCode("BG", "Bulgaria", "+359", "flag_bg.png"),
            new CountryCode("BH", "Bahrain", "+973", "flag_bh.png"),
            new CountryCode("BI", "Burundi", "+257", "flag_bi.png"),
            new CountryCode("BJ", "Benin", "+229", "flag_bj.png"),
            new CountryCode("BL", "Saint Barthélemy", "+590", "flag_bl.png"),
            new CountryCode("BM", "Bermuda", "+1", "flag_bm.png"),
            new CountryCode("BN", "Brunei Darussalam", "+673", "flag_bn.png"),
            new CountryCode("BO", "Bolivia", "+591", "flag_bo.png"), // , Plurinational State of
            new CountryCode("BQ", "Bonaire", "+599", "flag_bq.png"),
            new CountryCode("BR", "Brazil", "+55", "flag_br.png"),
            new CountryCode("BS", "Bahamas", "+1", "flag_bs.png"),
            new CountryCode("BT", "Bhutan", "+975", "flag_bt.png"),
            new CountryCode("BV", "Bouvet Island", "+47", "flag_bv.png"),
            new CountryCode("BW", "Botswana", "+267", "flag_bw.png"),
            new CountryCode("BY", "Belarus", "+375", "flag_by.png"),
            new CountryCode("BZ", "Belize", "+501", "flag_bz.png"),
            new CountryCode("CA", "Canada", "+1", "flag_ca.png"),
            new CountryCode("CC", "Cocos (Keeling) Islands", "+61", "flag_cc.png"),
            new CountryCode("CD", "Congo", "+243", "flag_cd.png"), // , The Democratic Republic of the
            new CountryCode("CF", "Central African Republic", "+236", "flag_cf.png"),
            new CountryCode("CG", "Congo", "+242", "flag_cg.png"),
            new CountryCode("CH", "Switzerland", "+41", "flag_ch.png"),
            new CountryCode("CI", "Ivory Coast", "+225", "flag_ci.png"),
            new CountryCode("CK", "Cook Islands", "+682", "flag_ck.png"),
            new CountryCode("CL", "Chile", "+56", "flag_cl.png"),
            new CountryCode("CM", "Cameroon", "+237", "flag_cm.png"),
            new CountryCode("CN", "China", "+86", "flag_cn.png"),
            new CountryCode("CO", "Colombia", "+57", "flag_co.png"),
            new CountryCode("CR", "Costa Rica", "+506", "flag_cr.png"),
            new CountryCode("CU", "Cuba", "+53", "flag_cu.png"),
            new CountryCode("CV", "Cape Verde", "+238", "flag_cv.png"),
            new CountryCode("CW", "Curacao", "+599", "flag_cw.png"),
            new CountryCode("CX", "Christmas Island", "+61", "flag_cx.png"),
            new CountryCode("CY", "Cyprus", "+357", "flag_cy.png"),
            new CountryCode("CZ", "Czech Republic", "+420", "flag_cz.png"),
            new CountryCode("DE", "Germany", "+49", "flag_de.png"),
            new CountryCode("DJ", "Djibouti", "+253", "flag_dj.png"),
            new CountryCode("DK", "Denmark", "+45", "flag_dk.png"),
            new CountryCode("DM", "Dominica", "+1", "flag_dm.png"),
            new CountryCode("DO", "Dominican Republic", "+1", "flag_do.png"),
            new CountryCode("DZ", "Algeria", "+213", "flag_dz.png"),
            new CountryCode("EC", "Ecuador", "+593", "flag_ec.png"),
            new CountryCode("EE", "Estonia", "+372", "flag_ee.png"),
            new CountryCode("EG", "Egypt", "+20", "flag_eg.png"),
            new CountryCode("EH", "Western Sahara", "+212", "flag_eh.png"),
            new CountryCode("ER", "Eritrea", "+291", "flag_er.png"),
            new CountryCode("ES", "Spain", "+34", "flag_es.png"),
            new CountryCode("ET", "Ethiopia", "+251", "flag_et.png"),
            new CountryCode("FI", "Finland", "+358", "flag_fi.png"),
            new CountryCode("FJ", "Fiji", "+679", "flag_fj.png"),
            new CountryCode("FK", "Falkland Islands (Malvinas)", "+500", "flag_fk.png"),
            new CountryCode("FM", "Micronesia", "+691", "flag_fm.png"), // , Federated States of
            new CountryCode("FO", "Faroe Islands", "+298", "flag_fo.png"),
            new CountryCode("FR", "France", "+33", "flag_fr.png"),
            new CountryCode("GA", "Gabon", "+241", "flag_ga.png"),
            new CountryCode("GB", "United Kingdom", "+44", "flag_gb.png"),
            new CountryCode("GD", "Grenada", "+1", "flag_gd.png"),
            new CountryCode("GE", "Georgia", "+995", "flag_ge.png"),
            new CountryCode("GF", "French Guiana", "+594", "flag_gf.png"),
            new CountryCode("GG", "Guernsey", "+44", "flag_gg.png"),
            new CountryCode("GH", "Ghana", "+233", "flag_gh.png"),
            new CountryCode("GI", "Gibraltar", "+350", "flag_gi.png"),
            new CountryCode("GL", "Greenland", "+299", "flag_gl.png"),
            new CountryCode("GM", "Gambia", "+220", "flag_gm.png"),
            new CountryCode("GN", "Guinea", "+224", "flag_gn.png"),
            new CountryCode("GP", "Guadeloupe", "+590", "flag_gp.png"),
            new CountryCode("GQ", "Equatorial Guinea", "+240", "flag_gq.png"),
            new CountryCode("GR", "Greece", "+30", "flag_gr.png"),
            new CountryCode("GS", "South Georgia and the South Sandwich Islands", "+500", "flag_gs.png"),
            new CountryCode("GT", "Guatemala", "+502", "flag_gt.png"),
            new CountryCode("GU", "Guam", "+1", "flag_gu.png"),
            new CountryCode("GW", "Guinea-Bissau", "+245", "flag_gw.png"),
            new CountryCode("GY", "Guyana", "+595", "flag_gy.png"),
            new CountryCode("HK", "Hong Kong", "+852", "flag_hk.png"),
            new CountryCode("HM", "Heard Island and McDonald Islands", "", "flag_hm.png"),
            new CountryCode("HN", "Honduras", "+504", "flag_hn.png"),
            new CountryCode("HR", "Croatia", "+385", "flag_hr.png"),
            new CountryCode("HT", "Haiti", "+509", "flag_ht.png"),
            new CountryCode("HU", "Hungary", "+36", "flag_hu.png"),
            new CountryCode("ID", "Indonesia", "+62", "flag_id.png"),
            new CountryCode("IE", "Ireland", "+353", "flag_ie.png"),
            new CountryCode("IL", "Israel", "+972", "flag_il.png"),
            new CountryCode("IM", "Isle of Man", "+44", "flag_im.png"),
            new CountryCode("IN", "India", "+91", "flag_in.png"),
            new CountryCode("IO", "British Indian Ocean Territory", "+246", "flag_io.png"),
            new CountryCode("IQ", "Iraq", "+964", "flag_iq.png"),
            new CountryCode("IR", "Iran, Islamic Republic of", "+98", "flag_ir.png"),
            new CountryCode("IS", "Iceland", "+354", "flag_is.png"),
            new CountryCode("IT", "Italy", "+39", "flag_it.png"),
            new CountryCode("JE", "Jersey", "+44", "flag_je.png"),
            new CountryCode("JM", "Jamaica", "+1", "flag_jm.png"),
            new CountryCode("JO", "Jordan", "+962", "flag_jo.png"),
            new CountryCode("JP", "Japan", "+81", "flag_jp.png"),
            new CountryCode("KE", "Kenya", "+254", "flag_ke.png"),
            new CountryCode("KG", "Kyrgyzstan", "+996", "flag_kg.png"),
            new CountryCode("KH", "Cambodia", "+855", "flag_kh.png"),
            new CountryCode("KI", "Kiribati", "+686", "flag_ki.png"),
            new CountryCode("KM", "Comoros", "+269", "flag_km.png"),
            new CountryCode("KN", "Saint Kitts and Nevis", "+1", "flag_kn.png"),
            new CountryCode("KP", "North Korea", "+850", "flag_kp.png"),
            new CountryCode("KR", "South Korea", "+82", "flag_kr.png"),
            new CountryCode("KW", "Kuwait", "+965", "flag_kw.png"),
            new CountryCode("KY", "Cayman Islands", "+345", "flag_ky.png"),
            new CountryCode("KZ", "Kazakhstan", "+7", "flag_kz.png"),
            new CountryCode("LA", "Lao People's Democratic Republic", "+856", "flag_la.png"),
            new CountryCode("LB", "Lebanon", "+961", "flag_lb.png"),
            new CountryCode("LC", "Saint Lucia", "+1", "flag_lc.png"),
            new CountryCode("LI", "Liechtenstein", "+423", "flag_li.png"),
            new CountryCode("LK", "Sri Lanka", "+94", "flag_lk.png"),
            new CountryCode("LR", "Liberia", "+231", "flag_lr.png"),
            new CountryCode("LS", "Lesotho", "+266", "flag_ls.png"),
            new CountryCode("LT", "Lithuania", "+370", "flag_lt.png"),
            new CountryCode("LU", "Luxembourg", "+352", "flag_lu.png"),
            new CountryCode("LV", "Latvia", "+371", "flag_lv.png"),
            new CountryCode("LY", "Libyan Arab Jamahiriya", "+218", "flag_ly.png"),
            new CountryCode("MA", "Morocco", "+212", "flag_ma.png"),
            new CountryCode("MC", "Monaco", "+377", "flag_mc.png"),
            new CountryCode("MD", "Moldova", "+373", "flag_md.png"), // , Republic of
            new CountryCode("ME", "Montenegro", "+382", "flag_me.png"),
            new CountryCode("MF", "Saint Martin", "+590", "flag_mf.png"),
            new CountryCode("MG", "Madagascar", "+261", "flag_mg.png"),
            new CountryCode("MH", "Marshall Islands", "+692", "flag_mh.png"),
            new CountryCode("MK", "Macedonia", "+389", "flag_mk.png"), // , The Former Yugoslav Republic of
            new CountryCode("ML", "Mali", "+223", "flag_ml.png"),
            new CountryCode("MM", "Myanmar", "+95", "flag_mm.png"),
            new CountryCode("MN", "Mongolia", "+976", "flag_mn.png"),
            new CountryCode("MO", "Macao", "+853", "flag_mo.png"),
            new CountryCode("MP", "Northern Mariana Islands", "+1", "flag_mp.png"),
            new CountryCode("MQ", "Martinique", "+596", "flag_mq.png"),
            new CountryCode("MR", "Mauritania", "+222", "flag_mr.png"),
            new CountryCode("MS", "Montserrat", "+1", "flag_ms.png"),
            new CountryCode("MT", "Malta", "+356", "flag_mt.png"),
            new CountryCode("MU", "Mauritius", "+230", "flag_mu.png"),
            new CountryCode("MV", "Maldives", "+960", "flag_mv.png"),
            new CountryCode("MW", "Malawi", "+265", "flag_mw.png"),
            new CountryCode("MX", "Mexico", "+52", "flag_mx.png"),
            new CountryCode("MY", "Malaysia", "+60", "flag_my.png"),
            new CountryCode("MZ", "Mozambique", "+258", "flag_mz.png"),
            new CountryCode("NA", "Namibia", "+264", "flag_na.png"),
            new CountryCode("NC", "New Caledonia", "+687", "flag_nc.png"),
            new CountryCode("NE", "Niger", "+227", "flag_ne.png"),
            new CountryCode("NF", "Norfolk Island", "+672", "flag_nf.png"),
            new CountryCode("NG", "Nigeria", "+234", "flag_ng.png"),
            new CountryCode("NI", "Nicaragua", "+505", "flag_ni.png"),
            new CountryCode("NL", "Netherlands", "+31", "flag_nl.png"),
            new CountryCode("NO", "Norway", "+47", "flag_no.png"),
            new CountryCode("NP", "Nepal", "+977", "flag_np.png"),
            new CountryCode("NR", "Nauru", "+674", "flag_nr.png"),
            new CountryCode("NU", "Niue", "+683", "flag_nu.png"),
            new CountryCode("NZ", "New Zealand", "+64", "flag_nz.png"),
            new CountryCode("OM", "Oman", "+968", "flag_om.png"),
            new CountryCode("PA", "Panama", "+507", "flag_pa.png"),
            new CountryCode("PE", "Peru", "+51", "flag_pe.png"),
            new CountryCode("PF", "French Polynesia", "+689", "flag_pf.png"),
            new CountryCode("PG", "Papua New Guinea", "+675", "flag_pg.png"),
            new CountryCode("PH", "Philippines", "+63", "flag_ph.png"),
            new CountryCode("PK", "Pakistan", "+92", "flag_pk.png"),
            new CountryCode("PL", "Poland", "+48", "flag_pl.png"),
            new CountryCode("PM", "Saint Pierre and Miquelon", "+508", "flag_pm.png"),
            new CountryCode("PN", "Pitcairn", "+872", "flag_pn.png"),
            new CountryCode("PR", "Puerto Rico", "+1", "flag_pr.png"),
            new CountryCode("PS", "Palestinian Territory, Occupied", "+970", "flag_ps.png"),
            new CountryCode("PT", "Portugal", "+351", "flag_pt.png"),
            new CountryCode("PW", "Palau", "+680", "flag_pw.png"),
            new CountryCode("PY", "Paraguay", "+595", "flag_py.png"),
            new CountryCode("QA", "Qatar", "+974", "flag_qa.png"),
            new CountryCode("RE", "Réunion", "+262", "flag_re.png"),
            new CountryCode("RO", "Romania", "+40", "flag_ro.png"),
            new CountryCode("RS", "Serbia", "+381", "flag_rs.png"),
            new CountryCode("RU", "Russia", "+7", "flag_ru.png"),
            new CountryCode("RW", "Rwanda", "+250", "flag_rw.png"),
            new CountryCode("SA", "Saudi Arabia", "+966", "flag_sa.png"),
            new CountryCode("SB", "Solomon Islands", "+677", "flag_sb.png"),
            new CountryCode("SC", "Seychelles", "+248", "flag_sc.png"),
            new CountryCode("SD", "Sudan", "+249", "flag_sd.png"),
            new CountryCode("SE", "Sweden", "+46", "flag_se.png"),
            new CountryCode("SG", "Singapore", "+65", "flag_sg.png"),
            new CountryCode("SH", "Saint Helena", "+290", "flag_sh.png"), // , Ascension and Tristan Da Cunha
            new CountryCode("SI", "Slovenia", "+386", "flag_si.png"),
            new CountryCode("SJ", "Svalbard and Jan Mayen", "+47", "flag_sj.png"),
            new CountryCode("SK", "Slovakia", "+421", "flag_sk.png"),
            new CountryCode("SL", "Sierra Leone", "+232", "flag_sl.png"),
            new CountryCode("SM", "San Marino", "+378", "flag_sm.png"),
            new CountryCode("SN", "Senegal", "+221", "flag_sn.png"),
            new CountryCode("SO", "Somalia", "+252", "flag_so.png"),
            new CountryCode("SR", "Suriname", "+597", "flag_sr.png"),
            new CountryCode("SS", "South Sudan", "+211", "flag_ss.png"),
            new CountryCode("ST", "Sao Tome and Principe", "+239", "flag_st.png"),
            new CountryCode("SV", "El Salvador", "+503", "flag_sv.png"),
            new CountryCode("SX", "Sint Maarten", "+1", "flag_sx.png"),
            new CountryCode("SY", "Syrian Arab Republic", "+963", "flag_sy.png"),
            new CountryCode("SZ", "Swaziland", "+268", "flag_sz.png"),
            new CountryCode("TC", "Turks and Caicos Islands", "+1", "flag_tc.png"),
            new CountryCode("TD", "Chad", "+235", "flag_td.png"),
            new CountryCode("TF", "French Southern Territories", "+262", "flag_tf.png"),
            new CountryCode("TG", "Togo", "+228", "flag_tg.png"),
            new CountryCode("TH", "Thailand", "+66", "flag_th.png"),
            new CountryCode("TJ", "Tajikistan", "+992", "flag_tj.png"),
            new CountryCode("TK", "Tokelau", "+690", "flag_tk.png"),
            new CountryCode("TL", "East Timor", "+670", "flag_tl.png"),
            new CountryCode("TM", "Turkmenistan", "+993", "flag_tm.png"),
            new CountryCode("TN", "Tunisia", "+216", "flag_tn.png"),
            new CountryCode("TO", "Tonga", "+676", "flag_to.png"),
            new CountryCode("TR", "Turkey", "+90", "flag_tr.png"),
            new CountryCode("TT", "Trinidad and Tobago", "+1", "flag_tt.png"),
            new CountryCode("TV", "Tuvalu", "+688", "flag_tv.png"),
            new CountryCode("TW", "Taiwan", "+886", "flag_tw.png"),
            new CountryCode("TZ", "Tanzania", "+255", "flag_tz.png"), // , United Republic of
            new CountryCode("UA", "Ukraine", "+380", "flag_ua.png"),
            new CountryCode("UG", "Uganda", "+256", "flag_ug.png"),
            new CountryCode("UM", "U.S. Minor Outlying Islands", "", "flag_um.png"),
            new CountryCode("US", "United States", "+1", "flag_us.png"),
            new CountryCode("UY", "Uruguay", "+598", "flag_uy.png"),
            new CountryCode("UZ", "Uzbekistan", "+998", "flag_uz.png"),
            new CountryCode("VA", "Holy See (Vatican City State)", "+379", "flag_va.png"),
            new CountryCode("VC", "Saint Vincent and the Grenadines", "+1", "flag_vc.png"),
            new CountryCode("VE", "Venezuela", "+58", "flag_ve.png"), // , Bolivarian Republic of
            new CountryCode("VG", "Virgin Islands, British", "+1", "flag_vg.png"),
            new CountryCode("VI", "Virgin Islands, U.S.", "+1", "flag_vi.png"),
            new CountryCode("VN", "Viet Nam", "+84", "flag_vn.png"),
            new CountryCode("VU", "Vanuatu", "+678", "flag_vu.png"),
            new CountryCode("WF", "Wallis and Futuna", "+681", "flag_wf.png"),
            new CountryCode("WS", "Samoa", "+685", "flag_ws.png"),
            new CountryCode("XK", "Kosovo", "+383", "flag_xk.png"),
            new CountryCode("YE", "Yemen", "+967", "flag_ye.png"),
            new CountryCode("YT", "Mayotte", "+262", "flag_yt.png"),
            new CountryCode("ZA", "South Africa", "+27", "flag_za.png"),
            new CountryCode("ZM", "Zambia", "+260", "flag_zm.png"),
            new CountryCode("ZW", "Zimbabwe", "+263", "flag_zw.png")
        };
    }
}