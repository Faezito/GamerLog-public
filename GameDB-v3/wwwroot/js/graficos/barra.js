function fn_bar_1_barra(id, xAxis_data, seriesName, series_data, decimais, cor) {

    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);
    xAxis_data = JSON.parse(xAxis_data);
    series_data = JSON.parse(series_data);

    var option = {
        title: {
            text: ''
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        toolbox: {
            show: true,
            orient: 'vertical',
            left: 'right',
            right: '0%',
            top: '0%',
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: true },
                restore: { show: true },
            },
            label: {
                show: true,
                position: 'insideLeft',
                formatter: function (params) {
                    var val = parseFloat(params.value);
                    return val.toLocaleString('pt-br', { minimumFractionDigits: decimais, maximumFractionDigits: decimais });
                }
            },
        },
        legend: { show: false },
        grid: {
            left: '1%',
            right: '0%',
            top: '0%',
            bottom: '0%',
            height: "100%",
            width: "99%",
            containLabel: true
        },
        xAxis: {
            show: false
        },
        yAxis: {
            type: 'category',
            data: xAxis_data
        },
        series: [
            {
                itemStyle: {
                    color: cor
                },
                name: seriesName,
                type: 'bar',
                label: {
                    show: true,
                    position: 'insideLeft',
                    formatter: function (params) {
                        var val = parseFloat(params.value);
                        return val.toLocaleString('pt-br', { minimumFractionDigits: decimais, maximumFractionDigits: decimais });
                    }
                },
                data: series_data
            }
        ]
    };

    myChart.setOption(option);
}

function fn_bar_fat_departamento(id, data_series_1, data_series_2, data_valor_porc_1, data_valor_porc_2, data_valor_total_1, data_valor_total_2, data_padding_esq, data_padding_dir, decimals) {
    let chartTurnover = id;
    let myChart = echarts.init(chartTurnover);
    let option;

    let screenWidth = window.innerWidth;

    if (screenWidth <= 768 && data_padding_dir != '15%') { // Tamanho típico de celular
        data_padding_dir = '25%'; // Reduz o padding para telas menores
    }

    option = {
        grid: {
            left: data_padding_esq,
            right: data_padding_dir, // Mantendo o espaço à direita, para graphic
            top: '0%',
            bottom: '5%'
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "white"
            }
        },
        xAxis: {
            type: 'value',
            min: 0,
            max: 100,
            axisTick: { show: false },
            axisLine: { show: false },
            splitLine: { show: false }
        },
        yAxis: {
            type: 'category',
            data: [data_series_1, data_series_2]
        },
        series: [
            {
                data: [
                    {
                        value: data_valor_porc_1,
                        itemStyle: { color: 'rgb(145, 204, 117)' },
                        label: {
                            show: true,
                            position: 'insideLeft',  // Valor à esquerda dentro da barra
                            color: 'white',
                            fontWeight: 'bold',
                            textShadowColor: '#000',
                            textShadowBlur: '3',
                            fontSize: 12, // Tamanho da fonte reduzido
                            formatter: function () {
                                return parseFloat(data_valor_porc_1).toLocaleString('pt-BR', {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 0
                                }) + ' %';
                            }
                        }
                    },
                    {
                        value: data_valor_porc_2,
                        itemStyle: { color: 'rgb(84, 112, 198)' },
                        label: {
                            show: true,
                            position: 'insideLeft',  // Valor à esquerda dentro da barra
                            color: 'white',
                            fontWeight: 'bold',
                            textShadowColor: '#000',
                            textShadowBlur: '3',
                            fontSize: 12, // Tamanho da fonte reduzido
                            formatter: function () {
                                return parseFloat(data_valor_porc_2).toLocaleString('pt-BR', {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 0
                                }) + ' %';
                            }
                        }
                    }
                ],
                type: 'bar',
                showBackground: true,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.2)'
                },
            }
        ],
    };

    myChart.setOption(option);

    myChart.resize({
        height: 60
    })

    let width = myChart.getWidth();
    let height = myChart.getHeight();

    myChart.setOption({
        graphic: [
            {
                type: 'text',
                style: {
                    text: parseFloat(data_valor_total_2).toLocaleString('pt-BR', {
                        minimumFractionDigits: decimals,
                        maximumFractionDigits: decimals
                    }),
                    fill: 'black',
                    font: '15px Montserrat, sans-serif',
                    textAlign: 'right',
                    verticalAlign: 'middle', // Alinhamento vertical
                    textRendering: 'optimizeLegibility',  // Melhor renderização de fontes em dispositivos móveis
                    '-webkit-font-smoothing': 'antialiased',  // Suaviza as fontes no iOS e navegadores WebKit
                },
                position: [width - 0, height / 2 - 12], // Movido para a direita
                z: 100
            },
            {
                type: 'text',
                style: {
                    text: parseFloat(data_valor_total_1).toLocaleString('pt-BR', {
                        minimumFractionDigits: decimals,
                        maximumFractionDigits: decimals
                    }),
                    fill: 'black',
                    font: '15px Montserrat, sans-serif',
                    textAlign: 'right',
                    verticalAlign: 'middle', // Alinhamento vertical
                    textRendering: 'optimizeLegibility',  // Melhor renderização de fontes em dispositivos móveis
                    '-webkit-font-smoothing': 'antialiased',  // Suaviza as fontes no iOS e navegadores WebKit
                },
                position: [width - 0, height / 2 + 17], // Movido para a direita
                z: 100
            }
        ]
    });

}

function fn_bar_fat_departamento2(id, data_series_1, data_series_2, data_valor_porc_1, data_valor_porc_2, data_valor_total_1, data_valor_total_2, data_padding_esq, data_padding_dir, decimals) {
    let chartTurnover = id;
    let myChart = echarts.init(chartTurnover);
    let option;

    let screenWidth = window.innerWidth;

    if (screenWidth <= 768 && data_padding_dir != '15%') { // Tamanho típico de celular
        data_padding_dir = '25%'; // Reduz o padding para telas menores
    }

    option = {
        grid: {
            left: data_padding_esq,
            right: data_padding_dir, // Mantendo o espaço à direita, para graphic
            top: '0%',
            bottom: '5%'
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "white"
            }
        },
        xAxis: {
            type: 'value',
            min: 0,
            max: 100,
            axisTick: { show: false },
            axisLine: { show: false },
            splitLine: { show: false },
            axisLine: {
                show: false
            },
            axisTick: {
                show: false
            }
        },
        yAxis: {
            type: 'category',
            data: [data_series_1, data_series_2],
            axisLabel: {
                fontSize: '1rem',
                color: '#333',
                fontWeight: 'normal',
                margin: 12,
                align: 'right',
                fontFamily: 'Montserrat'
            },
            axisLine: {
                show: false
            },
            axisTick: {
                show: false
            }
        },
        series: [
            {
                data: [
                    {
                        value: data_valor_porc_1,
                        itemStyle: { color: 'rgb(145, 204, 117)' },
                        label: {
                            show: true,
                            position: 'insideLeft',  // Valor à esquerda dentro da barra
                            color: 'white',
                            fontWeight: 'bold',
                            textShadowColor: '#000',
                            textShadowBlur: '3',
                            fontSize: 12, // Tamanho da fonte reduzido
                            formatter: function () {
                                return parseFloat(data_valor_porc_1).toLocaleString('pt-BR', {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 0
                                }) + ' %';
                            }
                        }
                    },
                    {
                        value: data_valor_porc_2,
                        itemStyle: { color: 'rgb(84, 112, 198)' },
                        label: {
                            show: true,
                            position: 'insideLeft',  // Valor à esquerda dentro da barra
                            color: 'white',
                            fontWeight: 'bold',
                            textShadowColor: '#000',
                            textShadowBlur: '3',
                            fontSize: 12, // Tamanho da fonte reduzido
                            formatter: function () {
                                return parseFloat(data_valor_porc_2).toLocaleString('pt-BR', {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 0
                                }) + ' %';
                            }
                        }
                    }
                ],
                type: 'bar',
                showBackground: true,
                barWidth: '70%',
                barCategoryGap: 20,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.2)'
                },
            }
        ],
    };

    myChart.setOption(option);

    myChart.resize({
        height: 60
    })

    let width = myChart.getWidth();
    let height = myChart.getHeight();

    myChart.setOption({
        graphic: [
            {
                type: 'text',
                style: {
                    text: parseFloat(data_valor_total_2).toLocaleString('pt-BR', {
                        minimumFractionDigits: decimals,
                        maximumFractionDigits: decimals
                    }),
                    fill: 'black',
                    font: '15px Montserrat, sans-serif',
                    textAlign: 'right',
                    verticalAlign: 'middle', // Alinhamento vertical
                    textRendering: 'optimizeLegibility',  // Melhor renderização de fontes em dispositivos móveis
                    '-webkit-font-smoothing': 'antialiased',  // Suaviza as fontes no iOS e navegadores WebKit
                },
                position: [width - 0, height / 2 - 12], // Movido para a direita
                z: 100
            },
            {
                type: 'text',
                style: {
                    text: parseFloat(data_valor_total_1).toLocaleString('pt-BR', {
                        minimumFractionDigits: decimals,
                        maximumFractionDigits: decimals
                    }),
                    fill: 'black',
                    font: '15px Montserrat, sans-serif',
                    textAlign: 'right',
                    verticalAlign: 'middle', // Alinhamento vertical
                    textRendering: 'optimizeLegibility',  // Melhor renderização de fontes em dispositivos móveis
                    '-webkit-font-smoothing': 'antialiased',  // Suaviza as fontes no iOS e navegadores WebKit
                },
                position: [width - 0, height / 2 + 17], // Movido para a direita
                z: 100
            }
        ]
    });

}


function fn_bar_sazonal(id, data_series_1, data_series_2, data_valor_total_1, data_valor_total_2, crescimento, decimals) {
    let chartTurnover = id;
    let myChart = echarts.init(chartTurnover);
    let option;

    let screenWidth = window.innerWidth;

    let data_padding_dir = '20%';
    if (screenWidth <= 768) { // Tamanho típico de celular
        data_padding_dir = '25%'; // Reduz o padding para telas menores
        data_series_1 = data_series_1.slice(-2);
        data_series_2 = data_series_1.slice(-2);
    }


    let tempTotal1 = parseFloat(data_valor_total_1);
    let tempTotal2 = parseFloat(data_valor_total_2);

    let tempTotal = tempTotal1 + tempTotal2;
    let porcValor1 = (tempTotal1 / tempTotal) * 100;
    let porcValor2 = 100 - porcValor1;

    option = {
        grid: {
            left: '7%',
            right: data_padding_dir, // Mantendo o espaço à direita, para graphic
            top: '0%',
            bottom: '5%'
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "white"
            }
        },
        xAxis: {
            type: 'value',
            min: 0,
            max: 100,
            axisTick: { show: false },
            axisLine: { show: false },
            splitLine: { show: false }
        },
        yAxis: {
            type: 'category',
            data: [data_series_1, data_series_2]
        },
        series: [
            {
                data: [
                    {
                        value: porcValor1,
                        itemStyle: { color: 'rgb(145, 204, 117)' },
                        label: {
                            show: true,
                            position: 'insideLeft',  // Valor à esquerda dentro da barra
                            color: 'white',
                            fontWeight: 'bold',
                            textShadowColor: '#000',
                            textShadowBlur: '3',
                            fontSize: 12, // Tamanho da fonte reduzido
                            formatter: function () {
                                return 'Crescimento de ' +
                                    parseFloat(crescimento).toLocaleString('pt-BR', {
                                        minimumFractionDigits: 0,
                                        maximumFractionDigits: 2
                                    }) + ' %';
                            }
                        }
                    },
                    {
                        value: porcValor2,
                        itemStyle: { color: 'rgb(84, 112, 198)' },
                        label: {
                            show: true,
                            position: 'insideLeft',  // Valor à esquerda dentro da barra
                            color: 'white',
                            fontWeight: 'bold',
                            textShadowColor: '#000',
                            textShadowBlur: '3',
                            fontSize: 12, // Tamanho da fonte reduzido
                            formatter: function () {
                                return '';
                            }
                        }
                    }
                ],
                type: 'bar',
                showBackground: true,
                backgroundStyle: {
                    color: 'rgba(180, 180, 180, 0.2)'
                },
            }
        ],
    };

    myChart.setOption(option);

    myChart.resize({
        height: 60
    })

    let width = myChart.getWidth();
    let height = myChart.getHeight();

    myChart.setOption({
        graphic: [
            {
                type: 'text',
                style: {
                    text: parseFloat(data_valor_total_2).toLocaleString('pt-BR', {
                        minimumFractionDigits: decimals,
                        maximumFractionDigits: decimals
                    }),
                    fill: 'black',
                    font: '15px Montserrat, sans-serif',
                    textAlign: 'right',
                    verticalAlign: 'middle', // Alinhamento vertical
                    textRendering: 'optimizeLegibility',  // Melhor renderização de fontes em dispositivos móveis
                    '-webkit-font-smoothing': 'antialiased',  // Suaviza as fontes no iOS e navegadores WebKit
                },
                position: [width - 0, height / 2 - 12], // Movido para a direita
                z: 100
            },
            {
                type: 'text',
                style: {
                    text: parseFloat(data_valor_total_1).toLocaleString('pt-BR', {
                        minimumFractionDigits: decimals,
                        maximumFractionDigits: decimals
                    }),
                    fill: 'black',
                    font: '15px Montserrat, sans-serif',
                    textAlign: 'right',
                    verticalAlign: 'middle', // Alinhamento vertical
                    textRendering: 'optimizeLegibility',  // Melhor renderização de fontes em dispositivos móveis
                    '-webkit-font-smoothing': 'antialiased',  // Suaviza as fontes no iOS e navegadores WebKit
                },
                position: [width - 0, height / 2 + 17], // Movido para a direita
                z: 100
            }
        ]
    });
}