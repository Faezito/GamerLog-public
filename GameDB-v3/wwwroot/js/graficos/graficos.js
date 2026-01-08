window.labelOption = window.labelOption || {
    show: true,
    position: 'insideBottom',
    distance: 10,
    align: 'left',
    verticalAlign: 'middle',
    rotate: 90,
    fontSize: 13,
    textStyle: {
        fontWeight: "bold"
    },
    formatter: function (valor) {
        return valor.value.toLocaleString('pt-br');
    }
};

window.tooltipOption = window.tooltipOption || {
    trigger: 'axis',
    axisPointer: {
        type: 'shadow'
    },
    valueFormatter: (value) => value.toLocaleString('pt-br')
};

window.grid = window.grid || {
    left: '0%',
    right: '0%',
    top: '10%',
    bottom: '20%',
    height: "auto",
    width: "auto",
    containLabel: true
};

function fn_col_dupla_rel_meta(id, realizado, meta) {
    let el = document.getElementById(id);
    let myChart = echarts.init(el);
    let option;

    option = {
        grid: grid,
        title: {
            text: 'title_text',
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "white",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "white"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: true,
            data: ['Realizado', 'Meta'],
            bottom: 0,
            padding: 0
        },
        xAxis: {
            type: 'category',
            data: [],
            axisTick: { show: true }
        },
        yAxis: [
            {
                show: false,
                type: 'value'
            }
        ],
        series: [
            {
                name: "Realizado",
                label: {
                    show: true,
                    color: 'white',
                    fontWeight: 'bold',
                    textShadowColor: '#000',
                    textShadowBlur: '3',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return parseFloat(valor.value).toLocaleString('pt-BR', {
                            currency: 'BRL',
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: [realizado],
                showBackground: true,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.4)'
                },
                textColor: 'blue',
                itemStyle: {
                    color: realizado > meta ? 'darkgreen' : 'darkred'
                }
            },
            {
                name: "Meta",
                label: {
                    show: true,
                    color: 'white',
                    fontWeight: 'bold',
                    textShadowColor: '#000',
                    textShadowBlur: '3',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return parseFloat(valor.value).toLocaleString('pt-BR', {
                            currency: 'BRL',
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: [meta],
                showBackground: true,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.4)'
                },
                itemStyle: {
                    color: 'rgb(119, 152, 191)'
                }
            }
        ]
    };

    myChart.setOption(option);
}

function fn_coluna_dupla(id, xAxis, dataCafe, dataLoja) {
    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        grid: grid,
        tooltip: tooltipOption,
        legend: {
            data: ['CAFÉ', 'LOJA'],
            bottom: "0px",
            label: {
                formatter: function (valor) {
                    return valor.value.toLocaleString('pt-br');
                }
            }
        },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar', 'stack'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: xAxis
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: [
            {
                name: 'CAFÉ',
                type: 'bar',
                barGap: 0,
                label: labelOption,
                emphasis: {
                    focus: 'series'
                },
                data: dataCafe
            },
            {
                name: 'LOJA',
                type: 'bar',
                barGap: 0,
                label: labelOption,
                emphasis: {
                    focus: 'series'
                },
                data: dataLoja
            }
        ]
    };
    option && myChart.setOption(option);
}

function fn_coluna_duplaV2(id, xAxis, _series) {
    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    let names = _series.map(o => o.name);
    const filtrado = _series.filter(({ name }, index) => !names.includes(name, index + 1));
    names = filtrado.map(o => o.name);

    option = {
        grid: grid,
        tooltip: tooltipOption,
        legend: {
            data: names, //_series.map(function (a, b, c) { return a.name; }),
            bottom: "0px",
            //label: {
            //    formatter: function (valor) {
            //        return valor.value.toLocaleString('pt-br');
            //    }
            //}
        },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            //label: {
            //    formatter: function (valor) {
            //        return valor.value.toLocaleString('pt-br');
            //    }
            //},
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar', 'stack'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: xAxis
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: _series
    };
    option && myChart.setOption(option);
}

function fn_coluna_dupla_faturamento_meta(id, xAxis, dataFaturamento, dataMeta) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        grid: grid,
        tooltip: tooltipOption,
        legend: {
            data: ['FATURAMENTO', 'META'],
            bottom: "0px",
            label: {
                formatter: function (valor) {
                    return valor.value.toLocaleString('pt-br');
                }
            }
        },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            label: {
                formatter: function (valor) {
                    return valor.value.toLocaleString('pt-br');
                }
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar', 'stack'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: xAxis
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: [
            {
                name: 'FATURAMENTO',
                type: 'bar',
                barGap: 0,
                label: labelOption,
                emphasis: {
                    focus: 'series'
                },
                data: dataFaturamento
            },
            {
                name: 'META',
                type: 'bar',
                barGap: 0,
                label: labelOption,
                emphasis: {
                    focus: 'series'
                },
                data: dataMeta
            }
        ]
    };
    option && myChart.setOption(option);
}

function fn_coluna_dupla_proporcao(id, xAxis_data, data1, data2) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        grid: grid,
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: xAxis_data
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: [
            {
                name: 'AMBIENTES',
                type: 'bar',
                barGap: 0,
                label: {
                    show: true,
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    rotate: 0,
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    },
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                },
                data: [
                    data1,
                    {
                        value: data2,
                        itemStyle: {
                            color: '#91cc75'
                        }
                    }]
            }
        ]
    };
    option && myChart.setOption(option);
}

function fn_coluna_dupla_proporcao2(id, xAxis_data, data1, data2) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        grid: grid,
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: xAxis_data
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: [
            {
                name: 'AMBIENTES',
                type: 'bar',
                barGap: 0,
                label: {
                    show: true,
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    rotate: 0,
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    },
                    formatter: function (valor) {
                        let total = data1 + data2;
                        let perc = valor.value * 100 / (total);
                        return `${perc.toFixed(0)}%\n${valor.value.toLocaleString('pt-br')}`;
                    }
                },
                data: [
                    data1,
                    {
                        value: data2,
                        itemStyle: {
                            color: '#91cc75'
                        }
                    }]
            }
        ]
    };
    option && myChart.setOption(option);
}


/*
Ex: 
    ar_legend_data = ['2023', '2024', 'Meta']
    ar_xAxis_data = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    ar_series_data_0 = [50148, 43676, 59707, 66088, 46581, 47742, 35074, 35760, 25426, 23212, 18548, 38587]

    ar_series_data_0 = dados do ano anterior;
    ar_series_data_1 = dados do ano pesquisa/atual;
    ar_series_data_2 = Metas;
*/

function fn_coluna_dupla_linha(id, ar_legend_data, ar_xAxis_data, ar_series_data_0, ar_series_data_1, ar_series_data_2) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;
    
    option = {
        color: ['#5470C6', '#91CC75', '#EE6666'],
        grid: grid,
        legend: {
            data: ar_legend_data
        },
        xAxis: [
            {
                type: 'category',
                axisTick: {
                    alignWithLabel: false
                },
                data: ar_xAxis_data
            }
        ],
        yAxis: [
            {
                show: false,
                type: 'value',
                name: ar_legend_data[0],
                position: 'right',
                alignTicks: false,
                axisLine: {
                    show: false,

                },
                axisLabel: {
                    formatter: '{value}'
                }
            },
            {
                show: false,
                type: 'value',
                name: ar_legend_data[1],
                position: 'right',
                alignTicks: false,
                offset: 80,
                axisLine: {
                    show: false,

                },
                axisLabel: {
                    formatter: '{value}'
                }
            },
            {
                show: false,
                type: 'value',
                name: ar_legend_data[2],
                position: 'left',
                alignTicks: false,
                axisLine: {
                    show: false,

                },
                axisLabel: {
                    formatter: '{value}'
                }
            }
        ],
        series: [
            {
                name: ar_legend_data[0],
                type: 'bar',
                data: ar_series_data_0,
                label: {
                    show: true,
                    position: 'insideBottom',
                    distance: 10,
                    align: 'left',
                    verticalAlign: 'middle',
                    rotate: 90,
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    },
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                }
            },
            {
                name: ar_legend_data[1],
                type: 'bar',
                data: ar_series_data_1,
                label: {
                    show: true,
                    position: 'insideBottom',
                    distance: 10,
                    align: 'left',
                    verticalAlign: 'middle',
                    rotate: 90,
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    },
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                }
            },
            {
                name: ar_legend_data[2],
                type: 'line',
                data: ar_series_data_2,
                label: {
                    show: true,
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    rotate: 0,
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    },
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                }
            }
        ]
    };




    option && myChart.setOption(option);
}

function fn_coluna_dupla_FATvsMETA(id, text1, data1, text2, data2) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    let cor = '#91cc75';

    if (data1 < data2) {
        cor = 'darkred';
    }

    option = {
        grid: grid,
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: [text1, text2]
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: [
            {
                name: 'AMBIENTES',
                type: 'bar',
                barGap: 0,
                label: {
                    show: true,
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'middle',
                    rotate: 0,
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    },
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                },
                data: [
                    {
                        value: data1,
                        itemStyle: {
                            color: cor
                        }
                    },
                    {
                        value: data2,
                        itemStyle: {
                            color: '#5470c6'
                        }
                    }]
            }
        ]
    };
    option && myChart.setOption(option);
}

function fn_coluna(id, xAxis, dataValor) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        toolbox: {
            showTitle: true,
            itemGap: 8,
            itemSize: 18,
            show: false,
            orient: 'vertical',
            //right: 0,
            left: 'right',
            top: 'center',
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: true },
                magicType: { show: true, type: ['line', 'bar', 'stack'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        grid: grid,
        tooltip: tooltipOption,
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: xAxis
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: false
                },
                splitLine: {
                    show: false
                }
            }
        ],
        series: [
            {
                //name: 'CAFÉ',
                type: 'bar',
                barGap: 0,
                label: labelOption,
                emphasis: {
                    focus: 'series'
                },
                data: dataValor
            }
        ]
    };
    option && myChart.setOption(option);
}

function fn_pizza(id, cafeValor, lojaValor) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        series: [
            {
                name: 'AMBIENTE',
                type: 'pie',
                label: {
                    formatter: function (a) {
                        let _value = parseFloat(a.value).toLocaleString('pt-br');
                        let _percent = parseFloat(a.percent).toLocaleString('pt-br');
                        return `${a.name}\nR$ ${_value}\nPERC: ${_percent}%`;
                    },
                    align: 'left',
                    position: 'inside',
                    borderWidth: 1,
                    borderRadius: 4
                },
                data: [
                    {
                        value: cafeValor,
                        name: 'CAFÉ'
                    },
                    {
                        value: lojaValor,
                        name: 'LOJA'
                    }
                ]
            }
        ]
    };

    option && myChart.setOption(option);

}

function fn_barra_lateral(id, yAxis, data, emOperacao, emImplantacao, outros, total) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        title: {
            text: `Total ativas: ${total} - Operação: ${emOperacao} - Implantações: ${emImplantacao} - Outros: ${outros}`,
            top: 0,
            textStyle: {
                fontSize: 12,
                lineHeight: 5,
                fontStyle: "normal",
                fontWeight: "bold"
            },
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        legend: {},
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'value',
            boundaryGap: [0, 0.01]
        },
        yAxis: {
            type: 'category',
            data: yAxis
        },
        series: [
            {
                name: '',
                type: 'bar',
                data: data,
                label: {
                    show: true,
                    position: 'insideLeft',
                    distance: 10,
                    align: 'left',
                    verticalAlign: 'middle',
                    fontSize: 13,
                    textStyle: {
                        fontWeight: "bold"
                    }
                }
            }
        ]
    };

    option && myChart.setOption(option);

}